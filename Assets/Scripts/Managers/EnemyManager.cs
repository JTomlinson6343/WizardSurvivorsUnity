using UnityEngine;

[System.Serializable]
struct Curve
{
    public AnimationCurve curve;
    public float min;
    public float max;

    public float Evaluate(float x, float alpha)
    {
        return min + (max-min) * curve.Evaluate(x/alpha);
    }
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager m_Instance;

    private GameObject m_PlayerReference;
    [SerializeField] private GameObject[] m_EnemyPrefabs;
    [SerializeField] private GameObject[] m_BossPrefabs;
    private float m_TotalSpawnProbability;

    private float m_NextSpawn = 0.0f;
    [SerializeField] private float m_SpawnCooldown = 1.0f;
    [SerializeField] private float m_SpawnRadius = 30.0f;

    private int m_EnemyCount;
    private int m_EnemiesKilledThisWave;
    private int m_SpawnLimit;
    private int m_EnemiesSpawnedThisWave;

    [SerializeField] Curve m_SpawnCurve;
    [SerializeField] Curve m_HealthCurve;

    readonly float kHealthConstant = 10f;
    readonly float kGracePeriodTime = 2f;
    readonly float kChampionChance = 0.01f;

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

        if (Player.m_Instance == null)  return;

        m_PlayerReference = Player.m_Instance.gameObject;
    }

    void Awake()
    {
        m_Instance = this;

        // Calculate the total spawn probability
        foreach (GameObject enemyPrefab in m_EnemyPrefabs)
        {
            Enemy enemy = enemyPrefab.GetComponent<Enemy>();
            m_TotalSpawnProbability += enemy.m_SpawnProbability;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) ProgressionManager.m_Instance.SpawnBoss();

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
        Gizmos.DrawWireSphere(m_PlayerReference.transform.position, m_SpawnRadius);
    }

    public void IncrementEnemiesKilled()
    {
        m_EnemiesKilledThisWave++;
        m_EnemyCount--;

        print("this wave kills: " + m_EnemiesKilledThisWave.ToString() +
            "   total wave spawns: " + m_SpawnLimit.ToString() +
            "   enemy count" + m_EnemyCount.ToString());
    }

    public void PurgeEnemies()
    {
        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void SpawnEnemy()
    {
        if (StateManager.GetCurrentState() != State.PLAYING)  return;

        float now = Time.realtimeSinceStartup;

        if (now < m_NextSpawn) return;

        m_NextSpawn = now + m_SpawnCooldown;

        GameObject newEnemy = CreateNewEnemy();

        if (Random.Range(0f, 1f) < kChampionChance * ProgressionManager.m_Instance.m_WaveCounter)
            newEnemy.GetComponent<Enemy>().MakeChampion();

        m_EnemyCount++;
        m_EnemiesSpawnedThisWave++;
    }

    // Returns the stats of a new enemy based on current wave with random prefab
    public GameObject CreateNewEnemy()
    {
        GameObject enemyToSpawn = null;

        // Generate a random number between 0 and the total probability
        float randomValue = Random.Range(0f, m_TotalSpawnProbability);

        // Select the enemy based on the generated random value
        foreach (GameObject enemyPrefab in m_EnemyPrefabs)
        {
            Enemy enemy = enemyPrefab.GetComponent<Enemy>();

            if (randomValue < enemy.m_SpawnProbability)
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

    public Boss SpawnBoss()
    {
        PurgeEnemies();
        GameObject boss = Instantiate(m_BossPrefabs[0]);
        boss.transform.position = Player.m_Instance.transform.position + new Vector3(0f, 3f);
        boss.transform.SetParent(transform);

        return boss.GetComponent<Boss>();
    }

    public void GracePeriod()
    {
        // Reset values
        m_EnemiesKilledThisWave = 0;
        m_EnemiesSpawnedThisWave = 0;
        Invoke(nameof(StartNewWave), kGracePeriodTime);
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
        m_SpawnLimit = Mathf.RoundToInt(m_SpawnCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter, 10));

        ProgressionManager.m_Instance.m_WaveCounter++;
        ProgressionManager.m_Instance.UpdateWaveLabel(ProgressionManager.m_Instance.m_WaveCounter);

        if (ProgressionManager.m_Instance.m_WaveCounter%5 == 0)
        {
            ProgressionManager.m_Instance.SpawnBoss();
            print("Boss wave started.");
            return;
        }

        print("Wave " + ProgressionManager.m_Instance.m_WaveCounter.ToString() + " started. " + m_SpawnLimit.ToString() + " enemies spawning.");

        print("Enemy HP this wave: " + GetEnemyHPForWave().ToString());
    }

    private float GetEnemyHPForWave()
    {
        return m_HealthCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter - 1, 100) * kHealthConstant;
    }
}