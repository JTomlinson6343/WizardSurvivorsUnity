using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEObject : MonoBehaviour
{
    [SerializeField] float m_TickTimer;
    public float m_DamageScaling;
    float m_LastHit = 0;
    bool m_CanDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float now = Time.realtimeSinceStartup;

        // Disable damaging if not enough time has passed since last tick
        if (now - m_LastHit < m_TickTimer)
        {
            m_CanDamage = false;
        }
        else
        {
            m_CanDamage = true;
        }

        m_LastHit = now;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_CanDamage)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                enemy.TakeDamageNoIFrames(m_DamageScaling * Player.m_Instance.GetStats().damage);
            }
        }
    }
}
