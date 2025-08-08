using TMPro;
using UnityEngine;

public enum QuestType
{
    ClickCookies,
    BuyItems,
    EarnCookies,
    UpgradeBuy
}

public class Quest : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text questText;

    [Header("Quest Data")]
    public QuestType questType;
    public string description;
    public int targetAmount;
    public int currentAmount = 0;

    [HideInInspector] public bool completed = false;

    public void InitQuest(QuestType type, string desc, int target)
    {
        questType = type;
        description = desc;
        targetAmount = target;
        currentAmount = 0;
        completed = false;
        UpdateUI();
    }

    // add progres for quest
    public void AddProgress(int amount)
    {
        if (completed) return;

        currentAmount += amount;
        if (currentAmount >= targetAmount)
        {
            completed = true;
            Debug.Log("Quest Completed: " + description);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (questText != null)
            questText.text = $"{description} ({currentAmount}/{targetAmount})";
    }
}
