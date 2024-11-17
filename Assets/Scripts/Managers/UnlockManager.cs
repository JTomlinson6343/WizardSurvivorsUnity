using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InternalAchievement
{
    public string name;
    public string id;

    public bool isUnlocked()
    {
        return SteamworksManager.isAchievementUnlocked(id);
    }

    public void Unlock()
    {
        SteamworksManager.UnlockAchievement(id);
    }
}

[System.Serializable]
public class Unlockable
{
    public string name;
    public bool unlocked;

    public void Unlock()
    {
        unlocked = true;
    }
}

[System.Serializable]
public class TrackedStat
{
    public string name;
    public float stat;
}

[System.Serializable]
public class UnlockCondition
{
    public string FormatConditionMessage()
    {
        return message + " (" + Mathf.Clamp(UnlockManager.GetTrackedStatWithName(trackedStatName).stat, 0f, condition).ToString() + "/" + condition.ToString() + ")";
    }

    public string name;
    public Sprite image;
    public string message;
    public float condition;
    public bool doFormatMessage;
    public string trackedStatName;
}
public class UnlockManager: MonoBehaviour
{
    public static UnlockManager m_Instance;

    [SerializeField] GameObject m_PopupPrefab;
    List<UnlockCondition> m_PopupQueue = new List<UnlockCondition>();

    public List<UnlockCondition> m_UnlockConditions;
    public List<InternalAchievement> m_Achievements;

    public static List<UnlockCondition> GetUnlockConditions() { return m_Instance.m_UnlockConditions; }
    public static UnlockCondition GetUnlockConditionWithName(string name) { return GetUnlockConditions().Find(x => x.name == name); }
    public static InternalAchievement GetAchievementWithName(string name) { return m_Instance.m_Achievements.Find(x => x.name == name); }
    public static Unlockable GetUnlockableWithName(string name) {
        Unlockable unlockable = m_Unlockables.Find(x => x.name == name);
        if (unlockable == null)
        {
            Unlockable newUnlockable = new Unlockable();
            newUnlockable.name = name;
            newUnlockable.unlocked = false;
            m_Unlockables.Add(newUnlockable);
            SaveManager.SaveToFile();
            return newUnlockable;
        }
        else
        {
            return unlockable;
        }
    }
    public static TrackedStat GetTrackedStatWithName(string name) { return m_TrackedStats.Find(x => x.name == name); }

    public static List<TrackedStat> m_TrackedStats = new List<TrackedStat>();
    public static List<Unlockable> m_Unlockables = new List<Unlockable>();

    private void Awake()
    {
        m_Instance = this;

        StartCoroutine(UnlockConditionCheckLoop());
    }

    public static void PopulateUnlockables()
    {
        m_Unlockables.Clear();
        foreach (UnlockCondition condition in m_Instance.m_UnlockConditions)
        {
            Unlockable unlockable = new Unlockable();
            unlockable.name = condition.name;
            unlockable.unlocked = false;
            m_Unlockables.Add(unlockable);
        }
    }

    public static void PopulateTrackedStats()
    {
        m_TrackedStats.Clear();
        foreach (UnlockCondition condition in m_Instance.m_UnlockConditions)
        {
            // If stat already exists, move on
            if (GetTrackedStatWithName(condition.trackedStatName) != null) continue;

            TrackedStat stat = new TrackedStat();
            stat.name = condition.trackedStatName;
            stat.stat = 0;
            m_TrackedStats.Add(stat);
        }
    }

    IEnumerator UnlockConditionCheckLoop()
    {
        while (true)
        {
            CheckUnlockConditions();
            yield return new WaitForSeconds(2.5f);
        }
    }

    public static void CheckUnlockConditions()
    {
        if (m_TrackedStats.Count == 0) return;

        CheckConditionIsEqualOrMore("iceDamageDealt", "Ice Mage");
        CheckConditionIsEqualOrLess("totalCooldown", "Lightning Mage");
        CheckConditionIsEqualOrMore("kills", "Solarium Skull");
        CheckConditionIsEqualOrMore("damage", "Orb of the Oracle");
        CheckConditionIsEqualOrMore("summonDamageDealt", "Emberfly Jar");
        m_Instance.ShowUnlockPopups();
    }

    static void CheckConditionIsEqualOrMore(string stat, string unlockName)
    {
        if (GetTrackedStatWithName(stat).stat >= GetUnlockConditionWithName(unlockName).condition && !GetUnlockableWithName(unlockName).unlocked)
        {
            SetUnlocked(unlockName);
        }
    }
    static void CheckConditionIsEqualOrLess(string stat, string unlockName)
    {
        if (GetTrackedStatWithName(stat).stat <= GetUnlockConditionWithName(unlockName).condition && !GetUnlockableWithName(unlockName).unlocked)
        {
            SetUnlocked(unlockName);
        }
    }

    public static void SetUnlocked(string unlockName)
    {
        if (GetUnlockableWithName(unlockName).unlocked) return;
        GetUnlockableWithName(unlockName).Unlock();
        GetAchievementWithName(unlockName)?.Unlock();
        m_Instance.QueueUnlockPopup(unlockName);
        SaveManager.SaveToFile();
    }

    void QueueUnlockPopup(string unlockName)
    {
        if (m_PopupQueue.Exists(x => x.name == unlockName)) return;

        m_PopupQueue.Add(GetUnlockConditionWithName(unlockName));
    }

    void ShowUnlockPopups()
    {
        if (m_PopupQueue.Count <= 0) return;

        UnlockCondition unlock = m_PopupQueue[0];

        GameObject popup = Instantiate(m_PopupPrefab);

        popup.GetComponentInChildren<TextMeshProUGUI>().text = unlock.name + " Unlocked!";
        popup.GetComponent<Image>().sprite = unlock.image;

        popup.transform.SetParent(transform);

        StartCoroutine(Utils.DelayedCall(2.34f, () =>
        {
            Destroy(popup);
            if (m_PopupQueue.Count > 0) m_PopupQueue.Remove(m_PopupQueue[0]);
            ShowUnlockPopups();
        }));
    }
}
