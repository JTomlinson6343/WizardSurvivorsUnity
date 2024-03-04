using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class IceElementalSkill : Skill
{
    [SerializeField] GameObject m_IceElementalPrefab;
    [SerializeField] GameObject m_SmokePrefab;
    public override void Init(SkillData data)
    {
        base.Init(data);

        GameObject iceElemental = Instantiate(m_IceElementalPrefab);
        GameObject smoke = Instantiate(m_SmokePrefab);

        iceElemental.transform.position = Player.m_Instance.transform.position;
        smoke.transform.position = iceElemental.transform.position;

        smoke.transform.localScale = iceElemental.transform.localScale * 2f;

        GetComponent<Ability>().UpdateTotalStats();
        iceElemental.GetComponent<Summon>().m_AbilitySource = GetComponent<Ability>();
    }
}
