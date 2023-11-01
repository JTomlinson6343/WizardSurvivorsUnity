using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner m_Instance;

    private GameObject m_PlayerReference;
    [SerializeField] private GameObject m_EnemyPrefab;

    private float m_NextSpawn = 0.0f;
    [SerializeField] private float m_SpawnCooldown = 1.0f;
    [SerializeField] private float m_SpawnRadius = 30.0f;

    private int m_EnemyCount;
    private int m_EnemiesKilledThisWave;
    private int m_SpawnLimit;

    [SerializeField] int m_InitialSpawnLimit;
    [SerializeField] private float m_CurveA;
    [SerializeField] private float m_CurveB;
    [SerializeField] private float m_CurveC;
    [SerializeField] private float m_MaxSpawnLimit;

    private Vector3 GetSpawnPosition()
    {
        Vector3 playerPos = Player.m_Instance.transform.position;

        Vector3 spawnPos = playerPos + new Vector3(Random.Range(-m_SpawnRadius, m_SpawnRadius), Random.Range(-m_SpawnRadius, m_SpawnRadius), 0).normalized * m_SpawnRadius;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING) { return; }

        if (m_EnemiesKilledThisWave >= m_SpawnLimit)
            StartNewWave();
        if (m_EnemyCount >= m_SpawnLimit) return;

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

        if (now > m_NextSpawn)
        {
            m_NextSpawn = now + m_SpawnCooldown;

            GameObject enemy = Instantiate(m_EnemyPrefab);
            enemy.transform.position = GetSpawnPosition();
            enemy.transform.SetParent(transform, false);
            m_EnemyCount++;
        }
    }

    private void StartNewWave()
    {
        m_EnemyCount = 0;
        m_EnemiesKilledThisWave = 0;

        ProgressionManager.m_Instance.m_WaveCounter++;

        m_SpawnLimit = m_InitialSpawnLimit + Mathf.RoundToInt(m_MaxSpawnLimit * (m_CurveA * Mathf.Pow(ProgressionManager.m_Instance.m_WaveCounter, 3) + m_CurveB * ProgressionManager.m_Instance.m_WaveCounter + m_CurveC)/1000f);
        ProgressionManager.m_Instance.UpdateWaveLabel(ProgressionManager.m_Instance.m_WaveCounter);

        print("Wave " + ProgressionManager.m_Instance.m_WaveCounter.ToString() + " started. " + m_SpawnLimit.ToString() + " enemies spawning.");
    }
}