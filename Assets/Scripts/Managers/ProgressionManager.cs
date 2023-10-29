using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager m_Instance;

    BasicBar xpBar;

    [SerializeField] GameObject m_WaveIndicator;
    [SerializeField] GameObject m_ScoreIndicator;
    [SerializeField] GameObject m_LevelIndicator;

    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] TextMeshProUGUI m_GameOverInfoLabel;

    float m_Score = 0;
    float m_Level = 1;
    float m_WaveCounter = 1;

    float m_CurrentXP = 0;
    float m_NextLevelXP;

    public float m_XPCurveGradient;
    public float m_XPCurveIntercept;

    //Data
    private int m_EnemiesKilled = 0;
    private int m_ChampionsKilled = 0;
    private int m_SkillPointsGained = 0;

    // State machine


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

    public void UpdateWaveIndicator(float wave)
    {
        m_WaveIndicator.GetComponent<TextMeshProUGUI>().text = "Wave: " + Mathf.RoundToInt(wave).ToString();
    }    
    public void UpdateScoreIndicator(float score)
    {
        m_ScoreIndicator.GetComponent<TextMeshProUGUI>().text = "Score: " + Mathf.RoundToInt(score).ToString();
    }    
    public void UpdateLevelIndicator(float level)
    {
        m_LevelIndicator.GetComponent<TextMeshProUGUI>().text = "Level: " + Mathf.RoundToInt(level).ToString();
    }

    public void AddScore(float score)
    {
        m_Score += score;
        UpdateScoreIndicator(m_Score);
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

    public void IncrementEnemyKills() { m_EnemiesKilled++; }
    public void IncrementChampionKills() { m_ChampionsKilled++; }

    public void GameOver()
    {
        m_GameOverInfoLabel.text = "Enemies Killed: " + m_EnemiesKilled.ToString() + "\n";
        if (m_ChampionsKilled > 0)
        {
            m_GameOverInfoLabel.text += "Champions Killed: " + m_ChampionsKilled.ToString() + "\n";
        }

        m_GameOverInfoLabel.text += "Skill Points Gained: " + m_SkillPointsGained.ToString();

        m_GameOverScreen.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
