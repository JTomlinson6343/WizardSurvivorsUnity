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

    public GameObject m_Character;

    public SkillTree m_SkillTree;

    public void SetUnlockState(bool locked)
    {
        m_Unlocked = locked;
        GetComponent<Button>().interactable = !locked;
    }
}
