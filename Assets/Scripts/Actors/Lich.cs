using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : Enemy
{
    [SerializeField] float m_MeleeRadius;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_MeleeRadius);
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StateManager.ChangeState(State.BOSS);
            Player.m_Instance.transform.position = Vector3.zero;
        }

        if (StateManager.GetCurrentState() != State.PLAYING)
        {
            return;
        }
    }

    private void Brain()
    {
        if (Player.m_Instance == null) return;

        float distToPlayer = Vector2.Distance(Player.m_Instance.transform.position, transform.position);

        if (distToPlayer < m_MeleeRadius)
        {
            // Stomp attack
        }
        else
        {
            // Ranged attack
        }
    }
}
