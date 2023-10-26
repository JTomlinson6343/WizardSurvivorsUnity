using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DamageType
{
    Fire,
    Frost,
    Lightning,
    Poison,
    Light,
    Physical
}

// An instance of damage dealt to an actor
public struct DamageInstanceData
{
    public DamageType damageType;
    public ActorType userType;
    public ActorType recieverType;
    public float amount;
    public bool didCrit;
    public bool didKill;
}

public class DamageManager : MonoBehaviour
{
    [SerializeField] GameObject m_DamageNumberPrefab;

    public static DamageManager m_Instance;
    public class DataEvent : UnityEvent<DamageInstanceData> { }

    public static DataEvent m_DamageInstanceEvent = new DataEvent();

    private void Awake()
    {
        m_Instance = this;
    }

    public void DamageInstance(ActorType sourceType, GameObject target, DamageType damageType, float damage, Vector2 pos, bool doIframes, bool doDamageNumbers)
    {
        Actor actorComponent = target.GetComponent<Actor>();
        DamageOutput damageOutput = 0;
        if (doIframes)
        {
            // Damage actor
            damageOutput = actorComponent.TakeDamage(damage);
        }
        else
        {
            actorComponent.TakeDamageNoIFrames(damage);
        }

        if (damageOutput >= DamageOutput.invalidHit && doDamageNumbers)
        {
            // Spawn damage numbers
            GameObject damageNumber = Instantiate(m_DamageNumberPrefab);
            damageNumber.transform.position = pos;
            damageNumber.GetComponent<FloatingDamage>().m_Colour = GetDamageNumberColor(damageType);
            damageNumber.GetComponent<FloatingDamage>().m_Damage = damage;

            // Invoke damage instance event
            DamageInstanceData di = new DamageInstanceData();
            di.damageType = damageType;
            di.userType = sourceType;
            di.recieverType = target.GetComponent<Actor>().m_ActorType;
            di.amount = damage;
            di.didKill = damageOutput == DamageOutput.wasKilled;
            di.didCrit = false; // Change this when crits are implemented

            m_DamageInstanceEvent.Invoke(di);
        }
    }

    private Color GetDamageNumberColor(DamageType damageType)
    {
        switch(damageType)
        {
            case DamageType.Fire: return new Color(1, 0.4f, 0);
            case DamageType.Frost: return Color.cyan;
            case DamageType.Light: return Color.white;
            case DamageType.Physical: return Color.red;
            case DamageType.Poison: return Color.green;
            case DamageType.Lightning: return Color.blue;
            default:
                return Color.white;
        }
    }
}
