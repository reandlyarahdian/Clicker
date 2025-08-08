using UnityEngine;

public class QuestListener : MonoBehaviour
{
    public QuestManager questManager;

    private float lastCookies = 0;
    public GameManager gameManager;

    void Start()
    {
        lastCookies = gameManager.count;
    }

    public void OnCookieClick()
    {
        questManager.ProgressQuest(QuestType.ClickCookies, 1);
    }

    public void OnItemBuy()
    {
        questManager.ProgressQuest(QuestType.BuyItems, 1);
    }

    public void OnUpgradeBuy()
    {
        questManager.ProgressQuest(QuestType.UpgradeBuy, 1);
    }

    void Update()
    {
        float earned = gameManager.count - lastCookies;
        if (earned > 0)
        {
            questManager.ProgressQuest(QuestType.EarnCookies, Mathf.RoundToInt(earned));
            lastCookies = gameManager.count;
        }
    }
}
