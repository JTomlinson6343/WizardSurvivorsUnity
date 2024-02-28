using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    public void AddDebuff(Actor actor, Debuff debuffData)
    {
        if (IsDebuffPresent(actor, debuffData))
            return;

        StartCoroutine(DebuffRoutine(actor, debuffData));
    }

    public void AddFireDebuff(Actor actor, Debuff debuffData)
    {
        AddDebuff(actor,debuffData);

    }

    IEnumerator DebuffRoutine(Actor actor, Debuff debuffData)
    {
        actor.m_Debuffs.Add(debuffData);

        while (debuffData.m_TimeLeft > 0 && actor)
        {
            debuffData.OnTick(actor);

            yield return new WaitForSeconds(debuffData.kTickRate);
        }

        actor.m_Debuffs.Remove(debuffData);
    }

    private bool IsDebuffPresent(Actor actor, Debuff debuffData)
    {
        foreach (Debuff debuff in actor.m_Debuffs)
        {
            if (debuff.kType == debuffData.kType)
            {
                debuff.RefreshTimer();
                return true;
            }
        }

        return false;
    }

}