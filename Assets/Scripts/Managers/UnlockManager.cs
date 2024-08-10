using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
public struct TrackedStats
{
    public float iceDamageDealt; // Total ice damage dealt
    public float totalCooldown; // Cooldown in one run. Resets after run
}

[System.Serializable]
public class UnlockCondition
{
    public string FormatConditionMessage(float current)
    {
        return message + " (" + current.ToString() + "/" + condition.ToString() + ")";
    }

    public string name;
    public Sprite image;
    public string message;
    public float condition;
}
public class UnlockManager: MonoBehaviour
{
    public static UnlockManager m_Instance;

    [SerializeField] GameObject m_PopupPrefab;
    List<UnlockCondition> m_PopupQueue = new List<UnlockCondition>();

    public List<UnlockCondition> m_UnlockConditions;

    public static List<UnlockCondition> GetUnlockConditions() { return m_Instance.m_UnlockConditions; }
    public static UnlockCondition GetUnlockConditionWithName(string name) { return GetUnlockConditions().Find(x => x.name == name); }
    public static Unlockable GetUnlockableWithName(string name) { return m_Unlockables.Find(x => x.name == name); }

    public static TrackedStats m_TrackedStats;
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

    IEnumerator UnlockConditionCheckLoop()
    {
        while (true)
        {
            CheckUnlockConditions();
            yield return new WaitForSeconds(3f);
        }
    }

    public static void CheckUnlockConditions()
    {
        if (m_TrackedStats.iceDamageDealt >= GetUnlockConditionWithName("Ice Mage").condition && !GetUnlockableWithName("Ice Mage").unlocked)
        {
            SetUnlocked("Ice Mage");
        }
        if (m_TrackedStats.totalCooldown <= GetUnlockConditionWithName("Lightning Mage").condition && !GetUnlockableWithName("Lightning Mage").unlocked)
        {
            SetUnlocked("Lightning Mage");
        }
        m_Instance.ShowUnlockPopups();
    }

    public static void SetUnlocked(string unlockName)
    {
        GetUnlockableWithName(unlockName).Unlock();
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
