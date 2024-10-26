using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombinedSkillTree : SkillTree
{
    private SkillTree[] m_Trees = new SkillTree[2];

    public override void Start() { }

    public override void TryUnlockAchievement() { }

    public void SetSkillTrees(SkillTree tree1, SkillTree tree2)
    {
        m_Trees[0] = tree1;
        m_Trees[1] = tree2;

        //m_CharacterColour = Color.Lerp(tree1.m_CharacterColour, tree2.m_CharacterColour, 0.5f);
        m_CharacterColour = Color.Lerp(tree2.m_CharacterColour, Color.white, 0.5f);
    }

    public override SkillIcon GetSkillIconWithID(SkillID id)
    {
        foreach (SkillIcon icon in m_Trees[0].GetComponentsInChildren<SkillIcon>())
        {
            if (icon.m_Data.id == id) return icon;
        }
        foreach (SkillIcon icon in m_Trees[1].GetComponentsInChildren<SkillIcon>())
        {
            if (icon.m_Data.id == id) return icon;
        }

        return null;
    }

    public override void PassEnabledSkillsToManager(bool dontClear)
    {
        m_Trees[0].PassEnabledSkillsToManager(false);
        m_Trees[1].PassEnabledSkillsToManager(true);
    }
}
