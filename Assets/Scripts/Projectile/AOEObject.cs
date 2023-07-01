using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEObject : MonoBehaviour
{
    [SerializeField] float m_TickTimer;
    public float m_DamageScaling;
    float m_LastTick = 0;
    public bool m_CanDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_CanDamage)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //Player.m_Instance.DamageInstance(collision.gameObject, m_DamageScaling, collision.transform.position,false);
            }
        }
    }
}
