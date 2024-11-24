using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
struct Curve
{
    public AnimationCurve curve;
    public float min;
    public float max;
    public float alpha;

    public float Evaluate(float x)
    {
        return min + (max-min) * curve.Evaluate(x * alpha);
    }
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager m_Instance;

    public static List<Enemy> m_Enemies = new List<Enemy>();

    private GameObject m_PlayerReference;
    [SerializeField] private GameObject[] m_EnemyPrefabs;
    [SerializeField] private GameObject[] m_BossPrefabs;
    private float m_TotalSpawnProbability;

    private float m_NextSpawn = 0.0f;
    [SerializeField] private float m_SpawnCooldown = 1.0f;
    [SerializeField] private float m_WaveTimer = 1.0f; // Time before a new wave starts prematurely
    public float m_SpawnRadius = 15.0f;

    private int m_EnemyCount;
    private int m_EnemiesKilledThisWave;
    private int m_SpawnLimit;
    private int m_EnemiesSpawnedThisWave;

    private float m_SpawnCooldownMult = 1f;

    [SerializeField] Curve m_SpawnCurve;
    [SerializeField] Curve m_SpawnCooldownCurve;
    [SerializeField] Curve m_HealthCurve;

    readonly int   kEnemyHardLimit = 300;
    readonly float kGracePeriodTime = 2f;
    readonly float kChampionChance = 0.01f;

    [SerializeField] GameObject[] m_SlimePrefabs;

    private Vector3 GetSpawnPosition()
    {
        if (!Player.m_Instance) return Vector3.negativeInfinity;

        Vector2 playerPos = Player.m_Instance.transform.position;

        Vector2 spawnPos = playerPos + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * m_SpawnRadius;

        return spawnPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewWave();
        StartCoroutine(SetEnemyDir());

        if (Player.m_Instance == null) return;

        m_PlayerReference = Player.m_Instance.gameObject;
    }

    void Awake()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_EnemyCount >= kEnemyHardLimit) return;

        if (m_EnemiesKilledThisWave >= m_SpawnLimit)
        {
            GracePeriod();
        }

        if (m_EnemiesSpawnedThisWave >= m_SpawnLimit) return;

        SpawnEnemy();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(Player.m_Instance.transform.position, m_SpawnRadius);
    }

    IEnumerator SetEnemyDir()
    {
        while (true)
        {
            if (Player.m_Instance == null)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            m_Enemies.ForEach((e) =>
            {
                if (!e.m_FollowPlayer) return;

                Vector2 dir = (Player.m_Instance.gameObject.transform.position - e.transform.position).normalized;

                e.SetAnimState(dir);
                e.m_MoveDir = dir;
            });

            yield return new WaitForSeconds(0.5f);
        }
    }

    private float CalculateSpawnProbability()
    {
        float totalProbability = 0f;
        // Calculate the total spawn probability
        foreach (GameObject enemyPrefab in m_EnemyPrefabs)
        {
            Enemy enemy = enemyPrefab.GetComponent<Enemy>();

            if (ProgressionManager.m_Instance.m_WaveCounter >= enemy.m_MinWave)
                totalProbability += enemy.m_SpawnProbability;
        }
        return totalProbability;
    }

    public void IncrementEnemiesKilled()
    {
        m_EnemiesKilledThisWave++;
        m_EnemyCount--;
    }

    public void PurgeEnemies()
    {
        StartCoroutine(KillEnemies());
    }

    private IEnumerator KillEnemies()
    {
        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            enemy.DestroyEnemy();
        }
        yield return null;
    }

    public void OnRespawn()
    {
        m_EnemyCount--;
        m_EnemiesSpawnedThisWave--;
    }

    private void SpawnEnemy()
    {
        if (StateManager.GetCurrentState() != StateManager.State.PLAYING)  return;

        float now = Time.realtimeSinceStartup;

        if (now < m_NextSpawn) return;

        m_NextSpawn = now + GetSpawnCooldown();

        GameObject newEnemy = CreateNewEnemy();

        if (Random.Range(0f, 1f) < kChampionChance * ProgressionManager.m_Instance.m_WaveCounter)
            newEnemy.GetComponent<Enemy>().MakeChampion();

        m_Enemies.Add(newEnemy.GetComponent<Enemy>());
        m_EnemyCount++;
        m_EnemiesSpawnedThisWave++;
    }

    // Returns the stats of a new enemy based on current wave with random prefab
    public GameObject CreateNewEnemy()
    {
        // Check wave if it is going to be a special wave
        switch (ProgressionManager.m_Instance.m_WaveCounter)
        {
            case 2:
                m_SpawnCooldownMult = 0.25f;
                return CreateNewEnemy(m_EnemyPrefabs.First(x => x.name == "BasicSkeleton"));
            case 7:
                m_SpawnCooldownMult = 0.5f;
                return CreateNewEnemy(m_EnemyPrefabs.First(x => x.name == "Goblin"));
            case 10:
                m_SpawnCooldownMult = 0.5f;
                return CreateNewEnemy(m_EnemyPrefabs.First(x => x.name == "Slime"));
            case 14:
                m_SpawnCooldownMult = 0.25f;
                return CreateNewEnemy(m_EnemyPrefabs.First(x => x.name == "BigSkeleton"));
            case 18:
                m_SpawnCooldownMult = 0.5f;
                return CreateNewEnemy(m_EnemyPrefabs.First(x => x.name == "Golem"));
            case 22:
                return CreateNewEnemy(m_EnemyPrefabs.First(x => x.name == "EyeMonster"));
        }

        GameObject enemyToSpawn = null;

        // Generate a random number between 0 and the total probability
        float randomValue = Random.Range(0f, m_TotalSpawnProbability);

        // Select the enemy based on the generated random value
        foreach (GameObject enemyPrefab in m_EnemyPrefabs)
        {
            Enemy enemy = enemyPrefab.GetComponent<Enemy>();

            if (randomValue < enemy.m_SpawnProbability && ProgressionManager.m_Instance.m_WaveCounter >= enemy.m_MinWave)
            {
                enemyToSpawn = enemyPrefab;
                break;
            }
            randomValue -= enemy.m_SpawnProbability;
        }
        GameObject newEnemy = CreateNewEnemy(enemyToSpawn);

        return newEnemy;
    }

    // Returns the stats of a new enemy based on current wave with given prefab
    public GameObject CreateNewEnemy(GameObject enemyPrefab)
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = GetSpawnPosition();
        newEnemy.transform.SetParent(transform, false);
        newEnemy.GetComponent<Enemy>().m_MaxHealth = GetEnemyHPForWave() * newEnemy.GetComponent<Enemy>().m_HealthModifier;

        return newEnemy;
    }

    public GameObject SpawnBabySlime(int variant)
    {
        return Instantiate(m_SlimePrefabs[variant]);
    }

    public Boss SpawnBoss()
    {
        GameObject boss = Instantiate(ChooseRandomBoss());
        boss.transform.position = Player.m_Instance.transform.position + new Vector3(0f, 3f);
        boss.transform.SetParent(transform);

        return boss.GetComponent<Boss>();
    }

    private GameObject ChooseRandomBoss()
    {
        return m_BossPrefabs[Random.Range(0, m_BossPrefabs.Length)];
    }

    public void GracePeriod()
    {
        GracePeriod(kGracePeriodTime);
    }
    public void GracePeriod(float time)
    {
        // Reset values
        m_EnemiesKilledThisWave = 0;
        m_EnemiesSpawnedThisWave = 0;
        Invoke(nameof(StartNewWave), time);
    }

    private void StartNewWave()
    {
        m_SpawnCooldownMult = 1f;
        m_SpawnLimit = Mathf.RoundToInt(m_SpawnCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter));

        ProgressionManager.m_Instance.m_WaveCounter++;
        ProgressionManager.m_Instance.UpdateWaveLabel(ProgressionManager.m_Instance.m_WaveCounter);

        m_TotalSpawnProbability = CalculateSpawnProbability();
        CancelInvoke(nameof(GracePeriod));
        CancelInvoke(nameof(StartNewWave));

        if (IsBossWave(ProgressionManager.m_Instance.m_WaveCounter))
        {
            ProgressionManager.m_Instance.PreBoss();
            print("Boss wave started.");
            return;
        }

        print("Wave " + ProgressionManager.m_Instance.m_WaveCounter.ToString() + " started. " + m_SpawnLimit.ToString() + " enemies spawning.");

        print("Enemy HP this wave: " + GetEnemyHPForWave().ToString());

        Invoke(nameof(GracePeriod), m_WaveTimer);
    }

    private bool IsBossWave(int wave)
    {
        return wave % 4 == 0;
    }

    private float GetEnemyHPForWave()
    {
        return m_HealthCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter - 1);
    }

    private float GetSpawnCooldown()
    {
        Debug.Log(m_SpawnCooldownCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter - 1));
        return m_SpawnCooldownCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter - 1) * m_SpawnCooldownMult;
    }
}