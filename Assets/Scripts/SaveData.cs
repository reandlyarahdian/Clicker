using System;

[Serializable]
public class SaveData
{
    // GameManager data
    public float count;
    public float extraCPS;
    public float extraClickMultiplier;
    public float lastIncomeValue;

    // StoreUpgrade data
    [Serializable]
    public struct StoreUpgradeData
    {
        public string upgradeName;
        public int level;
    }
    public StoreUpgradeData[] storeUpgrades;

    // ShopItem data
    [Serializable]
    public struct ShopItemData
    {
        public bool purchased;
        public int level;
    }
    public ShopItemData[] shopItems;

    // Quest data
    [Serializable]
    public struct QuestData
    {
        public QuestType questType;
        public string description;
        public int targetAmount;
        public bool completed;
    }
    public QuestData[] quests;
    public float nextRefreshTime;
}