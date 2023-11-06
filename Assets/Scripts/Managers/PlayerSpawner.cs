using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner m_Instance;
    public static GameObject m_Character;
    public GameObject m_Camera;

    [SerializeField] Vector2 m_Bounds;

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
        if (!(Mathf.Abs(Player.m_Instance.transform.position.x) > m_Bounds.x))
            m_Camera.transform.position = new Vector3(Player.m_Instance.transform.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z);

        if (!(Mathf.Abs(Player.m_Instance.transform.position.y) > m_Bounds.y))
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, Player.m_Instance.transform.position.y, m_Camera.transform.position.z);
    }
}