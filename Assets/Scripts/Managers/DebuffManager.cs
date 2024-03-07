using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager m_Instance;

    [SerializeField] GameObject m_FireParticlePrefab;
    [SerializeField] GameObject m_SnowflakeParticlePrefab;
    [SerializeField] GameObject m_IceCrystalPrefab;

    private void Awake()
    {
        m_Instance = this;
    }

    public void AddDebuff(Actor actor, Debuff debuffData)
    {
        if (!actor) return;

        if (RefreshCheck(actor, debuffData))
            return;

        switch (debuffData.m_Type)
        {
            case Debuff.DebuffType.Blaze:
                StartCoroutine(FireDebuffRoutine(actor, debuffData));
                break;
            case Debuff.DebuffType.Frozen:
            case Debuff.DebuffType.Paralysed:
                StartCoroutine(FrozenDebuffRoutine(actor, debuffData));
                break;
            case Debuff.DebuffType.FrozenSkill:
                StartCoroutine(FrozenSkillDebuffRoutine(actor, debuffData));
                break;
            case Debuff.DebuffType.Frostbite:
                StartCoroutine(FrostbiteDebuffRoutine(actor, debuffData));
                break;
            default:
                StartCoroutine(DebuffRoutine(actor, debuffData));
                Debug.Log("Debuff with no type started");
                break;
        }
    }

    IEnumerator DebuffRoutine(Actor actor, Debuff debuffData)
    {
        actor.m_Debuffs.Add(debuffData);

        debuffData.OnApply(actor);

        while (debuffData.m_TimeLeft > 0 && actor)
        {
            debuffData.OnTick(actor);

            yield return new WaitForSeconds(debuffData.kTickRate);
        }

        if (actor)
        {
            debuffData.OnEnd(actor);
            actor.m_Debuffs.Remove(debuffData);
        }
    }

    IEnumerator FireDebuffRoutine(Actor actor, Debuff debuffData)
    {
        GameObject flames = Instantiate(m_FireParticlePrefab);
        flames.transform.SetParent(actor.transform);
        flames.transform.position = actor.m_DebuffPlacement.transform.position;

        yield return DebuffRoutine(actor,debuffData);

        Destroy(flames);
    }

    IEnumerator FrozenDebuffRoutine(Actor actor, Debuff debuffData)
    {
        actor.ToggleStunned(true);

        yield return DebuffRoutine(actor, debuffData);

        if (!actor) yield break;

        actor.ToggleStunned(false);
    }

    IEnumerator FrozenSkillDebuffRoutine(Actor actor, Debuff debuffData)
    {
        GameObject ice = Instantiate(m_IceCrystalPrefab);
        ice.transform.position = actor.transform.position + new Vector3(0f,0.5f);

        yield return FrozenDebuffRoutine(actor, debuffData);

        Destroy(ice);
    }

    IEnumerator FrostbiteDebuffRoutine(Actor actor, Debuff debuffData)
    {
        GameObject snowflakes = Instantiate(m_SnowflakeParticlePrefab);
        snowflakes.transform.SetParent(actor.transform);
        snowflakes.transform.position = actor.transform.position;

        yield return DebuffRoutine(actor, debuffData);

        Destroy(snowflakes);
    }

    public static Debuff GetDebuffIfPresent(Actor actor, Debuff.DebuffType type)
    {
        if (!actor) return null;
        if (actor.m_Debuffs == null) return null;

        foreach (Debuff debuff in actor.m_Debuffs)
        {
            if (debuff.m_Type == type)
            {
                return debuff;
            }
        }
        return null;
    }

    public bool RefreshCheck(Actor actor, Debuff debuffData) // Return true if debuff already present and gets refreshed
    {
        Debuff presentDebuff = GetDebuffIfPresent(actor, debuffData.m_Type);
        if (presentDebuff != null)
        {
            presentDebuff.RefreshTimer();
            return true;
        }
        return false;
    }
}