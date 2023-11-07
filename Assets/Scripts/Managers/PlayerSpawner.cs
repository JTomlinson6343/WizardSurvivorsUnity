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

    // Locks gameobject to bounds and outputs true if object is outside bounds
    public bool Contains(GameObject gameObject)
    {
        Vector3 pos = gameObject.transform.position;
        if (pos.x < left)
            return false;
        if (pos.x > right)
            return false;
        if (pos.y > top)
            return false;
        if (pos.y < bottom)
            return false;
        return true;
    }
}

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner m_Instance;
    public static GameObject m_Character;
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

        Instantiate(m_Character);
        m_Character.transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Player.m_Instance.transform.position.x > m_CameraBounds.left && Player.m_Instance.transform.position.x < m_CameraBounds.right)
            m_Camera.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z);

        if (Player.m_Instance.transform.position.y > m_CameraBounds.bottom && Player.m_Instance.transform.position.y < m_CameraBounds.top)
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, Player.m_Instance.transform.position.y, m_Camera.transform.position.z);
    }
}