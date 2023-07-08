using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [SerializeField] GameObject m_DamageNumberPrefab;

    public static DamageManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    public void DamageInstance(GameObject target, float damage, Vector2 pos, bool doIframes, bool doDamageNumbers)
    {
        Actor actorComponent = target.GetComponent<Actor>();
        bool validHit = true;
        if (doIframes)
        {
            // Damage actor
            validHit = actorComponent.TakeDamage(damage);
        }
        else
        {
            actorComponent.TakeDamageNoIFrames(damage);
        }

        if (validHit && doDamageNumbers)
        {
            // Spawn damage numbers
            GameObject damageNumber = Instantiate(m_DamageNumberPrefab);
            damageNumber.transform.position = pos;
            damageNumber.GetComponent<FloatingDamage>().m_Colour = Color.white;
            damageNumber.GetComponent<FloatingDamage>().m_Damage = damage;
        }
    }
}
