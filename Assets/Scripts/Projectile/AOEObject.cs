using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEObject : MonoBehaviour
{
    [HideInInspector] public Ability m_AbilitySource;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<Debuff>())
            {
                // If enemy is in the zone and doesnt yet have the debuff, add debuff 
                collision.gameObject.AddComponent<Debuff>().Init(-1f, m_AbilitySource.GetTotalStats().damage, m_AbilitySource.m_Info.damageType,Player.m_Instance.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If enemy leaves zone, remove debuff
            Debuff debuff = collision.gameObject.GetComponent<Debuff>();
            if (debuff)
            {
                Destroy(debuff);
            }
        }
    }
}
