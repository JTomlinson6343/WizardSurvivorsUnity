using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MultiMageCharacterPanel : MonoBehaviour
{
    [SerializeField] GameObject m_Menu;

    public CharacterIcon m_SelectedIcon;

    [SerializeField] MultiMageCharacterPanel m_OtherPanel;

    [SerializeField] GameObject m_CharIcon;
    [SerializeField] GameObject m_SkillTreeButton;
    [SerializeField] TextMeshProUGUI m_NameLabel;

    public void SetSelectedIcon(CharacterIcon icon)
    {
        m_SelectedIcon = icon;
        Debug.Log(icon.m_CharName + " selected");

        foreach (CharacterIcon ci in GetComponentsInChildren<CharacterIcon>())
        {
            ci.GetComponent<Image>().color = Color.white;
        }

        icon.GetComponent<Image>().color = CharacterMenu.m_Instance.m_HighlightColour;
    }

    public void OpenMenu()
    {
        foreach (CharacterIcon characterIcon in GetComponentsInChildren<CharacterIcon>())
        {
            characterIcon.GetComponent<Button>().interactable = true;
        }

        if (m_OtherPanel.m_SelectedIcon)
        {
            CharacterIcon dupeIcon = GetComponentsInChildren<CharacterIcon>().First( ci => ci.m_CharName == m_OtherPanel.m_SelectedIcon.m_CharName );
            dupeIcon.GetComponent<Button>().interactable = false;
        }
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        if (!m_SelectedIcon) return;

        m_SkillTreeButton.SetActive(true);
        m_CharIcon.GetComponent<Image>().sprite = m_SelectedIcon.GetComponent<Image>().sprite;
        m_NameLabel.text = m_SelectedIcon.m_CharName;
    }

    public void OpenSkillTree()
    {
        m_Menu.SetActive(false);

        m_SelectedIcon.m_SkillTree.gameObject.SetActive(true);
        m_SelectedIcon.m_SkillTree.GetComponent<Navigator2D>().Start();
    }
 }
