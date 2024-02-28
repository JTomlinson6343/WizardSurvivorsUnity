using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAOE : AOEObject
{
    [SerializeField] protected DebuffType m_Type;
    [SerializeField] bool m_DoPercentHealth;
    [SerializeField] int m_MaxStacks;
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        if (Debuff_old.IsDebuffPresent(collision.gameObject, m_Type)) return;

        // If enemy is in the zone and doesnt yet have the debuff, add debuff 
        AOEDebuff aoeDebuff = collision.gameObject.AddComponent<AOEDebuff>();
        aoeDebuff.Init(-1f, m_AbilitySource.GetTotalStats().damage, m_AbilitySource.m_Data.damageType, Player.m_Instance.gameObject, m_DoPercentHealth, m_MaxStacks, m_Type);
        aoeDebuff.m_AbilitySource = m_AbilitySource;
    }

    virtual public void OnTriggerExit2D(Collider2D collision)
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
