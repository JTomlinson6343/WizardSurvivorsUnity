using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager m_Instance;

    float m_Score;
    float m_Level;

    float m_CurrentXP;
    float m_NextLevelXP;

    public float m_XPCurveGradient;
    public float m_XPCurveIntercept;

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    public void AddScore(int score)
    {

    }

    public bool AddXP(int xp)
    {
        BasicBar xpBar = GetComponentInChildren<BasicBar>();

        // Increase XP
        m_CurrentXP += xp;

        if (m_CurrentXP >= m_NextLevelXP)
        {
            // If current xp is enough to level up, reset xp + excess
            m_CurrentXP -= m_NextLevelXP;

            m_NextLevelXP = m_XPCurveGradient*Mathf.Pow(m_Level,3) + m_XPCurveIntercept;
        }

        xpBar.UpdateSize(m_CurrentXP, m_NextLevelXP);
        return false;
    }

}
