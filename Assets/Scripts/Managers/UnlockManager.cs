using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Unlockables
{
    public bool iceMage;
    public bool lightningMage;
}

[System.Serializable]
public struct TrackedStats
{
    public float iceDamageDealt; // Total ice damage dealt
    public float totalCooldown; // Cooldown in one run. Resets after run
}

[System.Serializable]
public struct UnlockCondition
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

    public static TrackedStats m_TrackedStats;
    public static Unlockables  m_Unlockables;

    private void Awake()
    {
        m_Instance = this;

        StartCoroutine(UnlockConditionCheckLoop());
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
        if (m_TrackedStats.iceDamageDealt >= GetUnlockConditionWithName("Ice Mage").condition && !m_Unlockables.iceMage)
        {
            m_Unlockables.iceMage = true;
            m_Instance.QueueUnlockPopup("Ice Mage");
            SaveManager.SaveToFile();
        }
        if (m_TrackedStats.totalCooldown <= GetUnlockConditionWithName("Lightning Mage").condition && !m_Unlockables.lightningMage)
        {
            m_Unlockables.lightningMage = true;
            m_Instance.QueueUnlockPopup("Lightning Mage");
            SaveManager.SaveToFile();
        }
        m_Instance.ShowUnlockPopups();
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
