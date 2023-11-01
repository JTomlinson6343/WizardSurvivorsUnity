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
        Vector3 playerPos = m_PlayerReference.transform.position;

        Vector3 spawnPos = playerPos + new Vector3(Random.Range(-m_SpawnRadius, m_SpawnRadius), Random.Range(-m_SpawnRadius, m_SpawnRadius), 0).normalized * m_SpawnRadius;

        return spawnPos;
    }

    // Start is called before the first frame update
    void Start()
    {
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

        float now = Time.realtimeSinceStartup;

        if (now > m_NextSpawn)
        {
            m_NextSpawn = now + m_SpawnCooldown;

            GameObject enemy = Instantiate(m_EnemyPrefab);
            enemy.transform.position = GetSpawnPosition();
            enemy.transform.SetParent(transform, false);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_PlayerReference.transform.position, m_SpawnRadius);
    }

    public Vector2 GetClosestEnemyPos(Vector2 pos)
    {
        float minDist = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            // Calculate distance from enemy
            float dist = Vector3.Distance(enemy.transform.position, pos);
            if (dist < minDist)
            {
                // If enemy is closer than the closest enemy so far, set closest enemy to this
                minDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy.transform.position;
    }

    public Vector2 GetDirectionToEnemy(Vector2 pos)
    {
        return (GetClosestEnemyPos(pos)-pos).normalized;
    }

    public void PurgeEnemies()
    {
        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void StartNewWave()
    {
        int currentWave = ProgressionManager.m_Instance.m_WaveCounter;

        currentWave++;

        m_SpawnLimit = m_InitialSpawnLimit + Mathf.RoundToInt(m_MaxSpawnLimit * (m_CurveA * Mathf.Pow(currentWave, 3) + m_CurveB * currentWave + m_CurveC));
    }
}