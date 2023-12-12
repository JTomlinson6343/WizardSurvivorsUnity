using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAOE : AOEObject
{
    [SerializeField] DebuffType m_Type;
    [SerializeField] bool m_DoPercentHealth;
    [SerializeField] int m_MaxStacks;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<AOEDebuff>())
            {
                // If enemy is in the zone and doesnt yet have the debuff, add debuff 
                AOEDebuff aoeDebuff = collision.gameObject.AddComponent<AOEDebuff>();
                aoeDebuff.Init(-1f, m_AbilitySource.GetTotalStats().damage, m_AbilitySource.m_Data.damageType, Player.m_Instance.gameObject, m_DoPercentHealth, m_MaxStacks, m_Type);
                aoeDebuff.m_AbilitySource = m_AbilitySource;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If enemy leaves zone, remove debuff
            foreach (AOEDebuff debuff in collision.gameObject.GetComponents<AOEDebuff>())
            {
                if (debuff.m_DebuffType == m_Type) Destroy(debuff);
            }
        }
    }

    override public void OnTriggerEnter2D(Collider2D collision) { }
}
