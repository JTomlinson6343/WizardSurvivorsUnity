using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class CharacterIcon : MonoBehaviour
{
    public bool m_Unlocked;

    public string m_CharName;

    [TextArea(3, 10)]
    public string m_Description;

    public GameObject m_Character;

    public SkillTree m_SkillTree;
}
