using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalPlayerBuffSkill : Skill
{
    [SerializeField] PlayerStats m_StatBuffs;

    [SerializeField] float m_ResistanceBuffs;

    public override void Init(SkillData data)
    {
        base.Init(data);
        StartCoroutine(ConditionCheck(data));
    }

    private IEnumerator ConditionCheck(SkillData data)
    {
        bool lastCheck = false;
        bool thisCheck;
        while (true)
        {
            thisCheck = Condition();

            if (lastCheck != thisCheck)
            {
                if (Condition())
                {
                    for (int i = 0; i < data.level; i++)
                    {
                        Player.m_Instance.AddBonusStats(m_StatBuffs);
                        Player.m_Instance.m_DamageResistance += m_ResistanceBuffs;
                    }
                }
                else
                {
                    for (int i = 0; i < data.level; i++)
                    {
                        Player.m_Instance.RemoveBonusStats(m_StatBuffs);
                        Player.m_Instance.m_DamageResistance -= m_ResistanceBuffs;
                    }
                }
            }

            lastCheck = thisCheck;

            yield return new WaitForSeconds(1f);
        }
    }

    virtual protected bool Condition()
    {
        // For performance reason this might want to be changed to an event
        Player player = Player.m_Instance;
        return player.m_Health / player.m_MaxHealth <= 0.3f;
    }
}
