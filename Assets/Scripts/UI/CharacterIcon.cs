using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour
{
    public bool m_Unlocked;

    public string m_CharName;

    [TextArea(3, 10)]
    public string m_Description;

    public string m_StartSpellName;
    public string m_StartSpellDesc;

    public GameObject m_Character;

    public SkillTree m_SkillTree;

    public void SetUnlockState(bool locked)
    {
        m_Unlocked = !locked;
        if (locked)
        {
            GetComponent<Image>().color = Color.grey;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
