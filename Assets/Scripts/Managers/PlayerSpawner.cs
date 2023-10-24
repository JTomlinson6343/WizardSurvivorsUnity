using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static GameObject m_Character;

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
}