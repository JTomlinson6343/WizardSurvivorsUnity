using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Unlockables
{
    public bool iceMage;
    public bool lightningMage;
}

[System.Serializable]
public struct TrackedStats
{
    public float iceDamageDealt;
    public float totalCooldown;
}

public struct UnlockCondition
{
    public UnlockCondition(float _condition, string _message)
    {
        condition = _condition;
        message = _message;
    }

    public string FormatConditionMessage(float current)
    {
        return message + " (" + current.ToString() + "/" + condition.ToString() + ")";
    }

    public string message;
    public float condition;
}
public class UnlockManager
{
    public static UnlockCondition kIceMageCondition = new UnlockCondition(100000f, "To Unlock: Deal 100000 frost damage.");
    public static UnlockCondition kLightningMageCondition = new UnlockCondition(-0.7f, "To Unlock: Reach a total of 70% spell cooldown reduction in a single run.");

    public static TrackedStats m_TrackedStats;
    public static Unlockables  m_Unlockables;

    public static void CheckUnlockConditions()
    {
        if (m_TrackedStats.iceDamageDealt >= kIceMageCondition.condition && !m_Unlockables.iceMage)
        {
            m_Unlockables.iceMage = true;
            SaveManager.SaveToFile();
        }
        if (m_TrackedStats.totalCooldown <= kLightningMageCondition.condition && !m_Unlockables.lightningMage)
        {
            m_Unlockables.lightningMage = true;
            SaveManager.SaveToFile();
        }
    }
}
