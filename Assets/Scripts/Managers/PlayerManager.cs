using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct PlayerBounds
{
    public float top;
    public float bottom;
    public float left;
    public float right;
}

public class PlayerManager : MonoBehaviour // Manager that controls the player in-game
{
    public static PlayerManager m_Instance;
    public static GameObject m_Character;
    public static SkillTree m_SkillTreeRef;
    public GameObject m_Camera;

    [SerializeField] PlayerBounds m_CameraBounds;
    [SerializeField] PlayerBounds m_WorldBounds;

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
        // Bind camera to camera bounds
        if (Player.m_Instance.transform.position.x > m_CameraBounds.left && Player.m_Instance.transform.position.x < m_CameraBounds.right)
            m_Camera.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z);

        if (Player.m_Instance.transform.position.y > m_CameraBounds.bottom && Player.m_Instance.transform.position.y < m_CameraBounds.top)
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, Player.m_Instance.transform.position.y, m_Camera.transform.position.z);

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

    public void SaveSkillPoints(int points)
    {
        m_SkillTreeRef.m_TotalSkillPoints += points;
        m_SkillTreeRef.m_CurrentSkillPoints += points;
        SaveManager.SaveToFile();
    }
}