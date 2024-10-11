using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager m_Instance;

    private static bool m_LastGamepadPluggedInState;

    //UI
    [SerializeField] BasicBar m_XPBar;

    [SerializeField] BasicBar m_BossHealthBar;
    [SerializeField] TextMeshProUGUI m_BossNameLabel;

    [SerializeField] GameObject m_Hud;
    [SerializeField] GameObject m_PauseMenu;

    [SerializeField] GameObject m_WaveLabel;
    [SerializeField] GameObject m_ScoreLabel;
    [SerializeField] GameObject m_LevelLabel;

    [SerializeField] GameObject m_GameOverScreen;
    [SerializeField] TextMeshProUGUI m_GameOverInfoLabel;

    //Wave system
    int m_Score = 0;
    int m_Level = 1;

    [HideInInspector] public int m_WaveCounter = 0;

    private readonly float kBossGracePeriodTime = 6f;
    private readonly float kHealthPickupChance = 0.001f;

    //Pickup
    [SerializeField] GameObject m_XPOrbPrefab;
    [SerializeField] GameObject m_HealthPickupPrefab;
    [SerializeField] GameObject m_SkillPointOrbPrefab;
    [SerializeField] GameObject m_GoldSkillPointOrbPrefab;

    public float m_GoldGemChance = 0f;
    [SerializeField] float m_XPSpawnRadius;
    private float m_NextXPSpawn;
    [SerializeField] Curve m_SpawnCooldown;

    readonly float kPickupMoveSpeed = 15f;

    //XP
    public float m_CurrentXP = 0;
    public float m_NextLevelXP;

    [SerializeField] Curve m_LevelCurve;

    //Data
    private int m_EnemiesKilled = 0;
    private int m_ChampionsKilled = 0;
    private int m_SkillPointsGained = 0;
    private float m_XPGained = 0;

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        CalculateNextLevelXP();

        m_XPBar.UpdateSize(m_CurrentXP, m_NextLevelXP);
    }

    private void Update()
    {
        if (m_LastGamepadPluggedInState && Gamepad.current == null) {
            if (StateManager.GetCurrentState() != StateManager.State.PAUSED)
            {
                m_PauseMenu.GetComponent<PauseMenu>().InitPauseMenu();
                StateManager.ChangeState(StateManager.State.PAUSED);
            }
        }

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.B))
                PreBoss();
            if (Input.GetKeyDown(KeyCode.K))
                Steamworks.SteamUserStats.AddStat("kills", 5000);
        }
        m_LastGamepadPluggedInState = Gamepad.current != null;

        SpawnXPRandomly();

        PauseMenuInput();
    }

    public void PauseMenuInput()
    {
        if (StateManager.GetCurrentState() == StateManager.State.UPGRADING || StateManager.GetCurrentState() == StateManager.State.GAME_OVER || StateManager.GetCurrentState() == StateManager.State.TUTORIAL || StateManager.GetCurrentState() == StateManager.State.REVIVING) return;

        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (StateManager.GetCurrentState() == StateManager.State.PAUSED)
        {
            SaveManager.SaveToFile();
            StateManager.UnPause();
        }
        else
        {
            m_PauseMenu.GetComponent<PauseMenu>().InitPauseMenu();
            StateManager.ChangeState(StateManager.State.PAUSED);
        }
        // Show pause menu if in paused state
        m_PauseMenu.GetComponent<PauseMenu>().ToggleMenu(StateManager.GetCurrentState() == StateManager.State.PAUSED);
    }

    public void ToggleHUD(bool toggle)
    {
        m_Hud.SetActive(toggle);
    }

    public void UpdateWaveLabel(int wave)
    {
        m_WaveLabel.GetComponent<TextMeshProUGUI>().text = "Wave: " + wave.ToString();
        Utils.PulseAnim(m_WaveLabel.GetComponent<RectTransform>(), 0.5f);
    }
    public void UpdateScoreLabel(int score)
    {
        m_ScoreLabel.GetComponent<TextMeshProUGUI>().text = "Gems: " + score.ToString();
        Utils.PulseAnim(m_ScoreLabel.GetComponent<RectTransform>(), 0.25f);
    }
    public void UpdateLevelLabel()
    {
        m_LevelLabel.GetComponent<TextMeshProUGUI>().text = "Level: " + Mathf.RoundToInt(m_Level).ToString();
        Utils.PulseAnim(m_LevelLabel.GetComponent<RectTransform>(), 0.5f);
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
            GameObject pickup = SpawnPickup(pickupPrefab, pos);
            Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
            if (!rb) return;

            float modifier = 1f + amount / 100f;

            // Fire pickup in a random direction
            rb.velocity = new Vector2(Random.Range(-kPickupMoveSpeed * modifier, kPickupMoveSpeed * modifier),
                Random.Range(-kPickupMoveSpeed * modifier, kPickupMoveSpeed * modifier));
        }
    }

    public GameObject SpawnPickup(GameObject pickupPrefab, Vector2 pos)
    {
        GameObject pickup = Instantiate(pickupPrefab);
        pickup.transform.position = pos;
        pickup.transform.SetParent(transform);

        return pickup;
    }

    public void SpawnXP(Vector2 pos, int amount)
    {
        if (Random.Range(0f,1f) < kHealthPickupChance)
        {
            SpawnPickup(m_HealthPickupPrefab, pos, 1);
        }
        else
        {
            SpawnPickup(m_XPOrbPrefab, pos, amount);
        }
    }
    public void SpawnSkillPoint(Vector2 pos, int amount)
    {
        SpawnSkillPointWithChance(pos, amount);
    }

    private void SpawnSkillPointWithChance(Vector2 pos, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject pickup;

            if (Random.Range(0f, 1f) < m_GoldGemChance) pickup = SpawnPickup(m_GoldSkillPointOrbPrefab, pos);
            else pickup = SpawnPickup(m_SkillPointOrbPrefab, pos);

            Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
            if (!rb) return;

            float modifier = 1f + amount / 100f;

            // Fire pickup in a random direction
            rb.velocity = new Vector2(Random.Range(-kPickupMoveSpeed * modifier, kPickupMoveSpeed * modifier),
                Random.Range(-kPickupMoveSpeed * modifier, kPickupMoveSpeed * modifier));
        }
    }

    private void SpawnXPRandomly()
    {
        if (StateManager.GetCurrentState() != StateManager.State.PLAYING) return;

        float now = Time.realtimeSinceStartup;

        if (now < m_NextXPSpawn) return;

        SpawnPickup(m_XPOrbPrefab, Player.m_Instance.transform.position + Utils.GetRandomDirectionV3() * m_XPSpawnRadius);
        
        m_NextXPSpawn = now + m_SpawnCooldown.Evaluate(m_WaveCounter);
    }
    #endregion

    public bool AddXP(float xp)
    {
        float addedXP = xp * (1f + Player.m_Instance.GetStats().xpMod);
        // Increase XP
        m_CurrentXP += addedXP;
        m_XPGained += addedXP;

        if (m_CurrentXP >= m_NextLevelXP)
        {
            OnLevelUp();
        }

        AudioManager.m_Instance.PlayXPSound();
        m_XPBar.UpdateSize(m_CurrentXP, m_NextLevelXP);
        return false;
    }

    public void AddSkillPoints(int points) {
        m_SkillPointsGained += points;
        AddScore(points);
    }

    private void OnLevelUp()
    {
        // If current xp is enough to level up, reset xp + excess
        m_CurrentXP -= m_NextLevelXP;

        m_Level++;

        CalculateNextLevelXP();

        UpdateLevelLabel();

        if (m_Level == 2 || m_Level == 3)
        {
            AbilityManager.m_Instance.ChoosePassiveAbility();
            return;
        }
        if (m_Level%6 == 0)
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
        m_NextLevelXP = (Mathf.RoundToInt(m_LevelCurve.Evaluate(m_Level-1)));
        print(m_NextLevelXP.ToString() + " XP to level" + (m_Level+1).ToString());

        print("XP spawn rate: " + m_SpawnCooldown.Evaluate(m_WaveCounter).ToString());
    }

    public void IncrementEnemyKills() { m_EnemiesKilled++; }
    public void IncrementChampionKills() { m_ChampionsKilled++; }

    public void GameOver()
    {
        StateManager.ForceChangeState(StateManager.State.GAME_OVER);

        m_PauseMenu.SetActive(false);
        m_Hud.SetActive(false);
        m_BossHealthBar.gameObject.SetActive(false);

        EnemyManager.m_Instance.PurgeEnemies();

        // Save skill points to file
        PlayerManager.m_Instance.SaveSkillPoints(m_SkillPointsGained);

        // Display game over screen
        m_GameOverInfoLabel.text = "";
        m_GameOverInfoLabel.text += "XP Gained: " + Mathf.RoundToInt(m_XPGained).ToString() + "\n";

        m_GameOverInfoLabel.text += "Skill Gems Collected: " + m_SkillPointsGained.ToString() + "\n";

        m_GameOverInfoLabel.text += "Enemies Killed: " + m_EnemiesKilled.ToString() + "\n";
        if (m_ChampionsKilled > 0)
        {
            m_GameOverInfoLabel.text += "Champions Killed: " + m_ChampionsKilled.ToString() + "\n";
        }

        m_GameOverScreen.SetActive(true);

        if (!SteamworksManager.failed) Steamworks.SteamUserStats.StoreStats();

        Time.timeScale = 0;
    }

    public void PreBoss()
    {
        StateManager.ChangeState(StateManager.State.PRE_BOSS);
        // Suck all xp
        SuckUpXP();
        // Alert player that the boss is coming
        // Kill all the enemies
        EnemyManager.m_Instance.PurgeEnemies();

        Invoke(nameof(SpawnBoss), kBossGracePeriodTime);
    }

    private void SuckUpXP()
    {
        foreach (Pickup xp in GetComponentsInChildren<Pickup>())
        {
            xp.m_StartedAttracting = true;
            xp.m_PullSpeed *= 2f;
        }
    }

    public void SpawnBoss()
    {
        StateManager.ChangeState(StateManager.State.BOSS);
        // Lock camera to boss arena
        PlayerManager.m_Instance.OnStartBossFight();
        Boss boss = EnemyManager.m_Instance.SpawnBoss();
        boss.BossFightInit();

        if (m_WaveCounter >= 8)
        {
            // Make the boss enraged
            boss.Enraged(m_WaveCounter/5);
        }
        // Play boss music
        AudioManager.m_Instance.PlayMusic(17, 0.2f);

        // Init the boss health bar
        m_BossHealthBar.m_Actor = boss;
        m_BossHealthBar.transform.parent.gameObject.SetActive(true);

        // Display the boss' name
        m_BossNameLabel.text = boss.m_BossName;
    }

    public void OnBossFightEnd()
    {
        // Hide boss healthbar and name plate
        m_BossHealthBar.transform.parent.gameObject.SetActive(false);

        // Play victory sound
        AudioManager.m_Instance.PlaySound(18);

        AudioManager.m_Instance.PlayMusic(3,1f);

        Invoke(nameof(BossFightEndDelay), kBossGracePeriodTime);
    }

    private void BossFightEndDelay()
    {
        // Spawn a new wave after a delay
        EnemyManager.m_Instance.GracePeriod();

        PlayerManager.m_Instance.m_ActorsToBind = null;

        StateManager.ChangeState(StateManager.State.PLAYING);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
