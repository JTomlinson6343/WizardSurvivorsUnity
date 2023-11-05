using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerAOE : AOEObject
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<FlamethrowerDebuff>())
            {
                // If enemy is in the zone and doesnt yet have the debuff, add debuff 
                FlamethrowerDebuff flamethrowerDebuff = collision.gameObject.AddComponent<FlamethrowerDebuff>();
                flamethrowerDebuff.Init(-1f, m_AbilitySource.GetTotalStats().damage, m_AbilitySource.m_Data.damageType,Player.m_Instance.gameObject,false,1);
                flamethrowerDebuff.m_AbilitySource = m_AbilitySource;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If enemy leaves zone, remove debuff
            Debuff debuff = collision.gameObject.GetComponent<FlamethrowerDebuff>();
            if (debuff)
            {
                Destroy(debuff);
            }
        }
    }

    override public void OnTriggerEnter2D(Collider2D collision) { }
}
