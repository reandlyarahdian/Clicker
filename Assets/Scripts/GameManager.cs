using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] StoreUpgrade[] storeUpgrades;
    [SerializeField] int updatesPerSecond = 5;

    [HideInInspector] public float count = 0;
    float extraCPS = 1;
    float extraClickMultiplier = 1;
    float nextTimeCheck = 1;
    float lastIncomeValue = 0;

    [Header("Listener")]
    public QuestListener questListener;

    private void Start() {
        UpdateUI();
    }

    void Update() {
        if (nextTimeCheck < Time.timeSinceLevelLoad) {
            IdleCalculate();
            nextTimeCheck = Time.timeSinceLevelLoad + (1f / updatesPerSecond);
        }
    }

    // calculate count persecond
    void IdleCalculate() {
        float sum = 0;
        foreach (var storeUpgrade in storeUpgrades)
        {
            sum += storeUpgrade.CalculateIncomePerSecond();
            storeUpgrade.UpdateUI();
        }
        lastIncomeValue = sum;
        count += (sum * extraCPS)/ updatesPerSecond;
        UpdateUI();
    }

    // called every click
    public void ClickAction() {
        count++;
        count += lastIncomeValue * 0.02f * extraClickMultiplier;
        questListener.OnCookieClick();
        UpdateUI();
    }

    // for every upgarde purchase
    public bool PurchaseAction(int cost) {
        if (count >= cost) {
            count -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }


    // update every bought item's effect
    public void ClickPerSecond(ShopItem item)
    {
        if (item.purchased == true)
        {
            extraCPS += item.globalCookiesPerSecond;
            extraClickMultiplier += item.globalClickMultiplier;
        }
        Debug.Log($"{extraCPS}, {extraClickMultiplier}");
    }

    // update ui every called
    void UpdateUI() {
        countText.text = Mathf.RoundToInt(count).ToString();
        incomeText.text = lastIncomeValue.ToString() + "/s";
    }

    public void OnSaveButtonClicked()
    {
        SaveGame();
    }

    public void OnLoadButtonClicked()
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            count = count,
            extraCPS = extraCPS,
            extraClickMultiplier = extraClickMultiplier,
            lastIncomeValue = lastIncomeValue,
            storeUpgrades = new SaveData.StoreUpgradeData[storeUpgrades.Length],
            shopItems = new SaveData.ShopItemData[0],
            quests = new SaveData.QuestData[0]
        };

        // Store upgrade data
        for (int i = 0; i < storeUpgrades.Length; i++)
        {
            data.storeUpgrades[i] = new SaveData.StoreUpgradeData
            {
                upgradeName = storeUpgrades[i].upgradeName,
                level = storeUpgrades[i].level
            };
        }

        // Let other components add their data
        if (questListener != null && questListener.questManager != null)
        {
            data = questListener.questManager.AddQuestData(data);
        }

        // Save all shop items (need to find them first)
        ShopItem[] allShopItems = FindObjectsOfType<ShopItem>();
        data.shopItems = new SaveData.ShopItemData[allShopItems.Length];
        for (int i = 0; i < allShopItems.Length; i++)
        {
            data.shopItems[i] = new SaveData.ShopItemData
            {
                purchased = allShopItems[i].purchased,
                level = allShopItems[i].level
            };
        }

        SaveSystem.SaveGame(data);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        // Load basic game data
        count = data.count;
        extraCPS = data.extraCPS;
        extraClickMultiplier = data.extraClickMultiplier;
        lastIncomeValue = data.lastIncomeValue;

        // Load store upgrades
        for (int i = 0; i < Mathf.Min(data.storeUpgrades.Length, storeUpgrades.Length); i++)
        {
            storeUpgrades[i].level = data.storeUpgrades[i].level;
            storeUpgrades[i].UpdateUI();
        }

        // Load shop items
        ShopItem[] allShopItems = FindObjectsOfType<ShopItem>();
        for (int i = 0; i < Mathf.Min(data.shopItems.Length, allShopItems.Length); i++)
        {
            allShopItems[i].purchased = data.shopItems[i].purchased;
            allShopItems[i].level = data.shopItems[i].level;
            if (allShopItems[i].purchased)
            {
                allShopItems[i].ApplyEffect();
            }
            allShopItems[i].UpdateUI();
        }

        // Load quests
        if (questListener != null && questListener.questManager != null)
        {
            questListener.questManager.LoadQuestData(data);
        }

        UpdateUI();
    }
}
