using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager m_Instance;

    static List<Skill> m_Skills = new();

    // Start is called before the first frame update
    void Start()
    {
        m_Instance = this;
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }
    public void ResetSkillsAdded()
    {
        m_Skills.Clear();
    }
    public void AddSkill(Skill skill)
    {
        m_Skills.Add(skill);
    }

    void OnDamageInstance(DamageInstanceData damageInstance)
    {
        foreach (Skill skill in m_Skills)
        {
            skill.OnDamageInstance(damageInstance);
        }
    }
}