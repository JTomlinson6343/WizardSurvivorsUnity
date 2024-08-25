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
    public bool IsInBounds(Vector2 pos, float modifier)
    {
        return pos.x > modifier * left && pos.x < modifier * right && pos.y > modifier * bottom && pos.y < modifier * top;
    }
}

public class PlayerManager : MonoBehaviour // Manager that controls the player in-game
{
    public static PlayerManager m_Instance;

    public Actor[] m_ActorsToBind;

    public static GameObject m_Character;
    public static SkillTree m_SkillTreeRef;
    public static SkillTree m_GlobalSkillTreeRef;
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
        AudioManager.m_Instance.PlayMusic(3);
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
        if (StateManager.GetCurrentState() == StateManager.State.BOSS ||
            (StateManager.GetPreviousState() == StateManager.State.BOSS && (StateManager.GetPreviousState() == StateManager.State.UPGRADING || StateManager.GetPreviousState() == StateManager.State.GAME_OVER)))
        {
            BindActorsToBossArena();
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

    private void BindActorsToBossArena()
    {
        if (m_ActorsToBind.Length <= 0)
        {
            BossArena(Player.m_Instance);
            return;
        }
        foreach (Actor actor in m_ActorsToBind)
        {
            BossArena(actor);
        }
    }

    private void BossArena(Actor actor)
    {
        if (!actor) return;

        // Bind actor to world bounds
        if (actor.transform.position.x < m_BossArenaBounds.left)
            actor.transform.position = new Vector3(m_BossArenaBounds.left, actor.transform.position.y, actor.transform.position.z);

        if (actor.transform.position.x > m_BossArenaBounds.right)
            actor.transform.position = new Vector3(m_BossArenaBounds.right, actor.transform.position.y, actor.transform.position.z);

        if (actor.transform.position.y < m_BossArenaBounds.bottom)
            actor.transform.position = new Vector3(actor.transform.position.x, m_BossArenaBounds.bottom, actor.transform.position.z);

        if (actor.transform.position.y > m_BossArenaBounds.top)
            actor.transform.position = new Vector3(actor.transform.position.x, m_BossArenaBounds.top, actor.transform.position.z);
    }

    public void SaveSkillPoints(int points)
    {
        int difference = m_SkillTreeRef.m_TotalSkillPoints + points - SkillTree.kSkillPointCap;
        int globalDifference = m_GlobalSkillTreeRef.m_TotalSkillPoints + points - 9999;

        m_SkillTreeRef.m_TotalSkillPoints = Mathf.Clamp(m_SkillTreeRef.m_TotalSkillPoints + points,0, SkillTree.kSkillPointCap);
        m_SkillTreeRef.m_CurrentSkillPoints += points;
        if (difference > 0) m_SkillTreeRef.m_CurrentSkillPoints -= difference;

        m_GlobalSkillTreeRef.m_TotalSkillPoints = Mathf.Clamp(m_GlobalSkillTreeRef.m_TotalSkillPoints + points, 0, 9999);
        m_GlobalSkillTreeRef.m_CurrentSkillPoints += points;
        if (globalDifference > 0) m_GlobalSkillTreeRef.m_CurrentSkillPoints -= globalDifference;

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

        Vector3 originalPos = m_Camera.transform.localPosition;

        while (elapsed < duration)
        {
            if (StateManager.IsGameplayStopped()) break;

            elapsed += Time.deltaTime;

            float x = (Random.Range(-1f,1f) / 1f) * magnitude;
            float y = (Random.Range(-1f, 1f) / 1f) * magnitude;

            m_Camera.transform.localPosition += new Vector3(x, y);

            yield return new WaitForEndOfFrame();
        }

        m_Camera.transform.localPosition = originalPos;
        
        yield return null;
    }
}