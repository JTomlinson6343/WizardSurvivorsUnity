using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMageCharacterPanel : MonoBehaviour
{
    public CharacterIcon m_SelectedIcon;

    public void SetSelectedIcon(CharacterIcon icon)
    {
        m_SelectedIcon = icon;
        Debug.Log(icon.m_CharName + " selected");
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
