using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum DebuffType
{
    None,
    Blaze,
    Blizzard,
    Flamethrower,
    FireElementalFlames,
    BlackHole,
    Frozen,
    Paralysed
}

public class Debuff : MonoBehaviour
{
    public float m_Duration;
    private float m_TickInterval = 0.25f;
    public float m_Damage;
    public DamageType m_DamageType;
    protected GameObject m_Source;
    public Ability m_AbilitySource;
    public DebuffType m_DebuffType;
    protected int m_StackAmount = 1;

    float m_LastTick;

    public static bool IsDebuffPresent(GameObject gameObject, DebuffType debuffType)
    {
        // Check if a debuff with the same type is already present
        foreach (Debuff debuff in gameObject.GetComponents<Debuff>())
        {
            if (debuff.m_DebuffType == debuffType) return true;
        }
        return false;
    }

    public virtual void Init(float debuffTime, float damage, DamageType damageType, GameObject source, bool percentHealth, int maxStacks, DebuffType debuffType)
    {
        m_Damage = damage;
        m_DebuffType = debuffType;

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

        m_Duration = debuffTime;
        m_DamageType = damageType;
        m_Source = source;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if(m_Duration > 0)
        {
            Invoke(nameof(EndDebuff), m_Duration);
        }
    }

    public void RefreshDebuffTimer(float newTime)
    {
        CancelInvoke();
        m_Duration = newTime;
        Invoke(nameof(EndDebuff), m_Duration);
    }

    public void IncrementStackAmount(int maxStacks)
    {
        if (m_StackAmount < maxStacks)
            m_StackAmount++;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING && StateManager.GetCurrentState() != State.BOSS) { return; }
        TickRoutine();
        DebuffBehaviour();
    }

    public DebuffType GetDebuffType()
    {
        return m_DebuffType;
    }

    virtual protected void TickRoutine()
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

    // Called once per tick of the debuff
    virtual protected void OnTick()
    {
        Vector2 pos = Vector2.zero;
        if (GetComponent<Actor>().m_DebuffPlacement)
        {
            pos = GetComponent<Actor>().m_DebuffPlacement.transform.position;
        }
        else
        {
            pos = transform.position;
        }
        pos += new Vector2(0.5f, 1f);

        DamageInstanceData data = new DamageInstanceData(m_Source,gameObject);
        data.amount = m_Damage*m_StackAmount;
        data.damageType = m_DamageType;
        data.isDoT = true;
        data.doDamageNumbers = true;
        data.doSoundEffect = false;
        DamageManager.m_Instance.DamageInstance(data, pos);
    }

    virtual protected void DebuffBehaviour() { }
}
