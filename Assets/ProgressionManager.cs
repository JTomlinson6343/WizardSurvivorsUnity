using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager m_Instance;

    BasicBar xpBar;

    float m_Score;
    float m_Level = 1;

    float m_CurrentXP = 0;
    float m_NextLevelXP;

    public float m_XPCurveGradient;
    public float m_XPCurveIntercept;

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        xpBar = GetComponentInChildren<BasicBar>();

        CalculateNextLevelXP();

        xpBar.UpdateSize(m_CurrentXP, m_NextLevelXP);

    }

    public void AddScore(float score)
    {

    }

    public bool AddXP(float xp)
    {
        // Increase XP
        m_CurrentXP += xp;

        if (m_CurrentXP >= m_NextLevelXP)
        {
            OnLevelUp();
        }

        xpBar.UpdateSize(m_CurrentXP, m_NextLevelXP);
        return false;
    }

    private void OnLevelUp()
    {
        // If current xp is enough to level up, reset xp + excess
        m_CurrentXP -= m_NextLevelXP;

        m_Level++;

        CalculateNextLevelXP();

        AbilityManager.m_Instance.ShowAbilityOptions();

    }

    private void CalculateNextLevelXP()
    {
        // Set next level xp
        m_NextLevelXP = m_XPCurveGradient * Mathf.Pow(m_Level, 3) + m_XPCurveIntercept;
    }
}
