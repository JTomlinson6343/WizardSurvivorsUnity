using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    public float m_DebuffTime;
    private float m_TickInterval = 0.25f;
    public float m_Damage;
    public DamageType m_DamageType;

    float m_LastTick;

    public void Init(float debuffTime, float damageScaling, DamageType damageType)
    {
        m_DebuffTime = debuffTime;
        m_Damage = damageScaling;
        m_DamageType = damageType;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(m_DebuffTime > 0)
        {
            Invoke(nameof(EndDebuff), m_DebuffTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TickRoutine();
    }

    void TickRoutine()
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastTick < m_TickInterval)
        {
            return;
        }
        OnTick();
        m_LastTick = now;
    }

    void EndDebuff()
    {
        Destroy(this);
    }

    virtual protected void OnTick()
    {
        DamageManager.m_Instance.DamageInstance(ActorType.Player,gameObject, m_DamageType, m_Damage,transform.position,false,true);
    }
}
