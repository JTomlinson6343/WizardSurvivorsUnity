using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOnDeathSkill : Skill
{
    [SerializeField] float m_Chance;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;
        if (!damageInstance.didKill) return;
        if (Random.Range(0f, 1f) >= m_Chance * m_Data.level) return;

        ProgressionManager.m_Instance.SpawnXP(damageInstance.target.transform.position, 1);
        AudioManager.m_Instance.PlayRandomPitchSound(31, 0.5f);
    }
}
