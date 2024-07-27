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
public class UnlockManager
{
    const float kIceMageCondition = 100000f;
    const float kLightningMageCondition = -0.7f;

    public static TrackedStats m_TrackedStats;
    public static Unlockables  m_Unlockables;

    public static void CheckUnlockConditions()
    {
        if (m_TrackedStats.iceDamageDealt >= kIceMageCondition && !m_Unlockables.iceMage)
        {
            m_Unlockables.iceMage = true;
            SaveManager.SaveToFile();
        }
        if (m_TrackedStats.totalCooldown <= kLightningMageCondition && !m_Unlockables.lightningMage)
        {
            m_Unlockables.lightningMage = true;
            SaveManager.SaveToFile();
        }
    }
}
