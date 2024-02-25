using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerBounds
{
    public float top;
    public float bottom;
    public float left;
    public float right;

    public bool IsInBounds(Vector2 pos)
    {
        return pos.x > left && pos.x < right && pos.y > bottom && pos.y < top;
    }
}

public class PlayerManager : MonoBehaviour // Manager that controls the player in-game
{
    public static PlayerManager m_Instance;

    public static GameObject m_Character;
    public static SkillTree m_SkillTreeRef;
    public GameObject m_Camera;
    [SerializeField] float m_CameraSpeed;

    [SerializeField] PlayerBounds m_CameraBounds;
    [SerializeField] PlayerBounds m_WorldBounds;

    public PlayerBounds m_InitialBossArenaBounds;
    public PlayerBounds m_BossArenaBounds;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (m_Character == null)
            return;

        // Spawn character passed in by character menu
        Instantiate(m_Character);
        m_Character.transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (StateManager.GetCurrentState() == State.BOSS ||
            (StateManager.GetPreviousState() == State.BOSS && (StateManager.GetPreviousState() == State.UPGRADING || StateManager.GetPreviousState() == State.GAME_OVER)))
        {
            BossArena();
            return;
        }

        // Bind camera to camera bounds
        if (Player.m_Instance.transform.position.x > m_CameraBounds.left && Player.m_Instance.transform.position.x < m_CameraBounds.right)
            m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position,
                new Vector3(Player.m_Instance.transform.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z), Time.deltaTime * m_CameraSpeed);

        if (Player.m_Instance.transform.position.y > m_CameraBounds.bottom && Player.m_Instance.transform.position.y < m_CameraBounds.top)
            m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position,
                new Vector3(m_Camera.transform.position.x, Player.m_Instance.transform.position.y, m_Camera.transform.position.z), Time.deltaTime * m_CameraSpeed);

        // Bind player to world bounds
        if (Player.m_Instance.transform.position.x < m_WorldBounds.left)
            Player.m_Instance.transform.position = new Vector3(m_WorldBounds.left, Player.m_Instance.transform.position.y, Player.m_Instance.transform.position.z);

        if (Player.m_Instance.transform.position.x > m_WorldBounds.right)
            Player.m_Instance.transform.position = new Vector3(m_WorldBounds.right, Player.m_Instance.transform.position.y, Player.m_Instance.transform.position.z);

        if (Player.m_Instance.transform.position.y < m_WorldBounds.bottom)
            Player.m_Instance.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_WorldBounds.bottom, Player.m_Instance.transform.position.z);

        if (Player.m_Instance.transform.position.y > m_WorldBounds.top)
            Player.m_Instance.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_WorldBounds.top, Player.m_Instance.transform.position.z);
    }

    private void BossArena()
    {
        // Bind player to world bounds
        if (Player.m_Instance.transform.position.x < m_BossArenaBounds.left)
            Player.m_Instance.transform.position = new Vector3(m_BossArenaBounds.left, Player.m_Instance.transform.position.y, Player.m_Instance.transform.position.z);

        if (Player.m_Instance.transform.position.x > m_BossArenaBounds.right)
            Player.m_Instance.transform.position = new Vector3(m_BossArenaBounds.right, Player.m_Instance.transform.position.y, Player.m_Instance.transform.position.z);

        if (Player.m_Instance.transform.position.y < m_BossArenaBounds.bottom)
            Player.m_Instance.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_BossArenaBounds.bottom, Player.m_Instance.transform.position.z);

        if (Player.m_Instance.transform.position.y > m_BossArenaBounds.top)
            Player.m_Instance.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_BossArenaBounds.top, Player.m_Instance.transform.position.z);
    }

    public void SaveSkillPoints(int points)
    {
        m_SkillTreeRef.m_TotalSkillPoints += points;
        m_SkillTreeRef.m_CurrentSkillPoints += points;
        SaveManager.SaveToFile();
    }

    public void OnStartBossFight()
    {
        m_BossArenaBounds.top    = Player.m_Instance.GetPosition().y + m_InitialBossArenaBounds.top;      // +
        m_BossArenaBounds.right  = Player.m_Instance.GetPosition().x + m_InitialBossArenaBounds.right;    // +
        m_BossArenaBounds.bottom = Player.m_Instance.GetPosition().y + m_InitialBossArenaBounds.bottom;   // -
        m_BossArenaBounds.left   = Player.m_Instance.GetPosition().x + m_InitialBossArenaBounds.left  ;   // -
    }

    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));

    }
    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
         
        while (elapsed < duration)
        {
            if (StateManager.IsGameplayStopped()) continue;

            elapsed += Time.deltaTime;

            float x = (Random.Range(-1f,1f) / 1f) * magnitude;
            float y = (Random.Range(-1f, 1f) / 1f) * magnitude;

            m_Camera.transform.localPosition += new Vector3(x, y);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}