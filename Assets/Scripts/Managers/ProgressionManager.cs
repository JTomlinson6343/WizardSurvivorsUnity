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

    //UI
    BasicBar xpBar;

    [SerializeField] GameObject m_Hud;

    [SerializeField] GameObject m_WaveLabel;
    [SerializeField] GameObject m_ScoreLabel;
    [SerializeField] GameObject m_LevelLabel;

    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] TextMeshProUGUI m_GameOverInfoLabel;

    //Wave system
    int m_Score = 0;
    int m_Level = 1;
    [HideInInspector] public int m_WaveCounter = 0;

    //Pickup
    [SerializeField] GameObject m_XPOrbPrefab;
    [SerializeField] GameObject m_SkillPointOrbPrefab;
    readonly float kPickupMoveSpeed = 15f;

    int m_CurrentXP = 0;
    int m_NextLevelXP;

    [SerializeField] Curve m_LevelCurve;

    //Data
    private int m_EnemiesKilled = 0;
    private int m_ChampionsKilled = 0;
    private int m_SkillPointsGained = 0;
    private int m_XPGained = 0;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            OnLevelUp();

        if (Input.GetKeyDown(KeyCode.P))
            AddSkillPoints(1);
    }

    public void ToggleHUD(bool toggle)
    {
        m_Hud.SetActive(toggle);
    }

    public void UpdateWaveLabel(int wave)
    {
        m_WaveLabel.GetComponent<TextMeshProUGUI>().text = "Wave: " + wave.ToString();
    }    
    public void UpdateScoreLabel(int score)
    {
        m_ScoreLabel.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }    
    public void UpdateLevelLabel()
    {
        m_LevelLabel.GetComponent<TextMeshProUGUI>().text = "Level: " + Mathf.RoundToInt(m_Level).ToString();
    }

    public void AddScore(int score)
    {
        m_Score += score;
        UpdateScoreLabel(m_Score);
    }

    #region Pickups
    public void SpawnPickup(GameObject pickupPrefab, Vector2 pos, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject pickup = Instantiate(pickupPrefab);
            pickup.transform.position = pos;
            Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
            if (!rb) return;

            // Fire pickup in a random direction
            rb.velocity = new Vector2(Random.Range(-kPickupMoveSpeed, kPickupMoveSpeed), Random.Range(-kPickupMoveSpeed, kPickupMoveSpeed));
        }
    }

    public void SpawnXP(Vector2 pos, int amount)
    {
        SpawnPickup(m_XPOrbPrefab, pos, amount);
    }
    public void SpawnSkillPoint(Vector2 pos, int amount)
    {
        SpawnPickup(m_SkillPointOrbPrefab, pos, amount);
    }
    #endregion

    public bool AddXP(int xp)
    {
        // Increase XP
        m_CurrentXP += xp;
        m_XPGained += xp;

        if (m_CurrentXP >= m_NextLevelXP)
        {
            OnLevelUp();
        }

        xpBar.UpdateSize(m_CurrentXP, m_NextLevelXP);
        return false;
    }

    public void AddSkillPoints(int points) { m_SkillPointsGained += points; }

    private void OnLevelUp()
    {
        // If current xp is enough to level up, reset xp + excess
        m_CurrentXP -= m_NextLevelXP;

        m_Level++;

        CalculateNextLevelXP();

        UpdateLevelLabel();

        if (m_Level == 2)
        {
            AbilityManager.m_Instance.ChoosePassiveAbility();
            return;
        }
        if (m_Level%5 == 0)
        {
            AbilityManager.m_Instance.ChoosePassiveAbility();
        }
        else
        {
            AbilityManager.m_Instance.ChooseBuffAbility();
        }
    }

    private void CalculateNextLevelXP()
    {
        // Set next level xp
        m_NextLevelXP = (Mathf.RoundToInt(m_LevelCurve.Evaluate(m_Level-1, 10)));
        print(m_NextLevelXP.ToString() + " XP to level" + (m_Level+1).ToString());
    }

    public void IncrementEnemyKills() { m_EnemiesKilled++; }
    public void IncrementChampionKills() { m_ChampionsKilled++; }

    public void GameOver()
    {
        StateManager.ChangeState(State.GAME_OVER);

        EnemySpawner.m_Instance.PurgeEnemies();

        // Save skill points to file
        PlayerManager.m_Instance.SaveSkillPoints(m_SkillPointsGained);

        // Display game over screen
        m_GameOverInfoLabel.text = "";
        m_GameOverInfoLabel.text += "XP Gained: " + m_XPGained.ToString() + "\n";

        m_GameOverInfoLabel.text += "Skill Points Gained: " + m_SkillPointsGained.ToString() + "\n";

        m_GameOverInfoLabel.text += "Enemies Killed: " + m_EnemiesKilled.ToString() + "\n";
        if (m_ChampionsKilled > 0)
        {
            m_GameOverInfoLabel.text += "Champions Killed: " + m_ChampionsKilled.ToString() + "\n";
        }

        m_GameOverScreen.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
