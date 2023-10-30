using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum DebuffType
{
    Blaze,
    Blizzard
}

public class Debuff : MonoBehaviour
{
    public float m_DebuffTime;
    private float m_TickInterval = 0.25f;
    public float m_Damage;
    public DamageType m_DamageType;
    protected GameObject m_Source;
    private DebuffType m_DebuffType;
    protected int m_StackAmount = 1;

    float m_LastTick;

    public virtual void Init(float debuffTime, float damage, DamageType damageType, GameObject source, bool percentHealth, int maxStacks)
    {
        m_Damage = damage;

        // Check that the debuff isn't already on the gameObject. If it is, refresh it.
        foreach (Debuff debuff in GetComponents<Debuff>())
        {
            // If debuff type is same as this debuff type
            if (debuff.m_DebuffType == m_DebuffType && debuff != this)
            {
                // Refresh debuff time
                debuff.RefreshDebuffTimer(debuffTime);
                debuff.IncrementStackAmount(maxStacks);

                EndDebuff();
            }
        }

        if (percentHealth)
            m_Damage *= GetComponent<Actor>().m_MaxHealth;

        m_DebuffTime = debuffTime;
        m_DamageType = damageType;
        m_Source = source;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if(m_DebuffTime > 0)
        {
            Invoke(nameof(EndDebuff), m_DebuffTime);
        }
    }

    public void RefreshDebuffTimer(float newTime)
    {
        CancelInvoke();
        m_DebuffTime = newTime;
        Invoke(nameof(EndDebuff), m_DebuffTime);
    }

    public void IncrementStackAmount(int maxStacks)
    {
        if (m_StackAmount < maxStacks)
            m_StackAmount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING) { return; }
        TickRoutine();
    }

    public DebuffType GetDebuffType()
    {
        return m_DebuffType;
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

    protected virtual void EndDebuff()
    {
        Destroy(this);
    }

    virtual protected void OnTick()
    {
        DamageInstanceData data = new DamageInstanceData(m_Source,gameObject);
        data.amount = m_Damage*m_StackAmount;
        data.damageType = m_DamageType;
        data.isDoT = true;
        data.doDamageNumbers = true;
        data.doSoundEffect = false;
        data.doIFrames = false;
        DamageManager.m_Instance.DamageInstance(data,transform.position);
    }
}
