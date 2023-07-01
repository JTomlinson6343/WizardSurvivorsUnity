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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<Debuff>())
            {
                collision.gameObject.AddComponent<Debuff>().Init(-1f, 0.001f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debuff debuff = collision.gameObject.GetComponent<Debuff>();
            if (debuff)
            {
                Destroy(debuff);
            }
        }
    }
}
