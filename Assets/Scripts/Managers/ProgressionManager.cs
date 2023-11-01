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

    [SerializeField] GameObject m_WaveLabel;
    [SerializeField] GameObject m_ScoreLabel;
    [SerializeField] GameObject m_LevelLabel;

    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] TextMeshProUGUI m_GameOverInfoLabel;

    int m_Score = 0;
    int m_Level = 1;
    [HideInInspector] public int m_WaveCounter = 0;

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

    public void UpdateWaveLabel(int wave)
    {
        m_WaveLabel.GetComponent<TextMeshProUGUI>().text = "Wave: " + wave.ToString();
    }    
    public void UpdateScoreLabel(int score)
    {
        m_ScoreLabel.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }    
    public void UpdateLevelLabel(int level)
    {
        m_LevelLabel.GetComponent<TextMeshProUGUI>().text = "Level: " + Mathf.RoundToInt(level).ToString();
    }

    public void AddScore(int score)
    {
        m_Score += score;
        UpdateScoreLabel(m_Score);
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
        StateManager.ChangeState(State.GAME_OVER);

        EnemySpawner.m_Instance.PurgeEnemies();

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
