using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner m_Instance;

    private GameObject m_PlayerReference;
    [SerializeField] private GameObject[] m_EnemyPrefabs;
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

    private Vector3 GetSpawnPosition()
    {
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
        if (StateManager.GetCurrentState() != State.PLAYING) { return; }

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
        float now = Time.realtimeSinceStartup;

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

        if (now > m_NextSpawn)
        {
            m_NextSpawn = now + m_SpawnCooldown;

            GameObject enemy = Instantiate(enemyToSpawn);
            enemy.transform.position = GetSpawnPosition();
            enemy.transform.SetParent(transform, false);
            enemy.GetComponent<Enemy>().m_MaxHealth = GetEnemyHPForWave() * enemy.GetComponent<Enemy>().m_HealthModifier;
            m_EnemyCount++;
            m_EnemiesSpawnedThisWave++;
        }
    }

    private void GracePeriod()
    {
        // Reset values
        m_EnemiesKilledThisWave = 0;
        m_EnemiesSpawnedThisWave = 0;
        Invoke(nameof(StartNewWave), kGracePeriodTime);
    }

    private void StartNewWave()
    {
        m_SpawnLimit = Mathf.RoundToInt(m_SpawnCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter, 10));

        ProgressionManager.m_Instance.m_WaveCounter++;
        ProgressionManager.m_Instance.UpdateWaveLabel(ProgressionManager.m_Instance.m_WaveCounter);

        print("Wave " + ProgressionManager.m_Instance.m_WaveCounter.ToString() + " started. " + m_SpawnLimit.ToString() + " enemies spawning.");

        print("Enemy HP this wave: " + GetEnemyHPForWave().ToString());
    }

    private float GetEnemyHPForWave()
    {
        return m_HealthCurve.Evaluate(ProgressionManager.m_Instance.m_WaveCounter - 1, 100) * kHealthConstant;
    }
}