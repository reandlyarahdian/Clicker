using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest[] quests;
    public float refreshInterval = 300f;

    private float nextRefreshTime;

    void Start()
    {
        AssignRandomQuests();
        nextRefreshTime = Time.time + refreshInterval;
    }

    void Update()
    {
        if (Time.time >= nextRefreshTime)
        {
            RefreshQuests();
        }
    }

    public void AssignRandomQuests()
    {
        List<(QuestType, string, int)> pool = new List<(QuestType, string, int)>()
        {
            (QuestType.ClickCookies, "Click cookie 50 times", 50),
            (QuestType.ClickCookies, "Click cookie 100 times", 100),
            (QuestType.ClickCookies, "Click cookie 500 times", 500),
            (QuestType.ClickCookies, "Click cookie 1000 times", 1000),
            (QuestType.ClickCookies, "Click cookie 5000 times", 5000),
            (QuestType.ClickCookies, "Click cookie 10000 times", 10000),
            (QuestType.ClickCookies, "Click cookie 50000 times", 50000),
            (QuestType.ClickCookies, "Click cookie 100000 times", 100000),
            (QuestType.BuyItems, "Buy 3 items", 3),
            (QuestType.BuyItems, "Buy 5 items", 5),
            (QuestType.BuyItems, "Buy 10 items", 10),
            (QuestType.BuyItems, "Buy 15 items", 15),
            (QuestType.BuyItems, "Buy 20 items", 20),
            (QuestType.BuyItems, "Buy 25 items", 25),
            (QuestType.BuyItems, "Buy 30 items", 30),
            (QuestType.BuyItems, "Buy 50 items", 50),
            (QuestType.EarnCookies, "Earn 200 cookies", 200),
            (QuestType.EarnCookies, "Earn 500 cookies", 500),
            (QuestType.EarnCookies, "Earn 2000 cookies", 2000),
            (QuestType.EarnCookies, "Earn 5000 cookies", 5000),
            (QuestType.EarnCookies, "Earn 20000 cookies", 20000),
            (QuestType.EarnCookies, "Earn 50000 cookies", 50000),
            (QuestType.UpgradeBuy, "Get 200 Upgrade", 200),
            (QuestType.UpgradeBuy, "Get 200 Upgrade", 500)
        };

        System.Random rand = new System.Random();

        foreach (Quest quest in quests)
        {
            var pick = pool[rand.Next(pool.Count)];
            quest.InitQuest(pick.Item1, pick.Item2, pick.Item3);
            pool.Remove(pick);
        }
    }

    // called every 5 minuites or every complited quest
    public void RefreshQuests()
    {
        AssignRandomQuests();
        nextRefreshTime = Time.time + refreshInterval;
    }

    public void ProgressQuest(QuestType type, int amount)
    {
        foreach (Quest quest in quests)
        {
            if (quest.questType == type && !quest.completed)
            {
                // add progres for quest
                quest.AddProgress(amount);
                if (quest.completed)
                {
                    RefreshQuests();
                    break;
                }
            }
        }
    }

    public SaveData AddQuestData(SaveData data)
    {
        data.quests = new SaveData.QuestData[quests.Length];
        for (int i = 0; i < quests.Length; i++)
        {
            data.quests[i] = new SaveData.QuestData
            {
                questType = quests[i].questType,
                description = quests[i].description,
                targetAmount = quests[i].targetAmount,
                completed = quests[i].completed
            };
        }
        data.nextRefreshTime = nextRefreshTime;
        return data;
    }

    public void LoadQuestData(SaveData data)
    {
        if (data.quests == null || data.quests.Length != quests.Length) return;

        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].InitQuest(
                data.quests[i].questType,
                data.quests[i].description,
                data.quests[i].targetAmount
            );
        }
        nextRefreshTime = data.nextRefreshTime;
    }
}
