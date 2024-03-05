using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject m_CharMenuRef;
    [SerializeField] GameObject m_OptionsMenuRef;

    [SerializeField] Button[] m_Buttons;

    int m_SelectedButtonPos;
    bool m_AxisInUse;

    private void Start()
    {
        SaveManager.LoadFromFile(m_CharMenuRef.GetComponent<CharacterMenu>().GetSkillTreeRefs());
        m_SelectedButtonPos = 0;
    }

    private void Update()
    {
        if (Gamepad.current != null)
            HandleInput();
    }

    private void HandleInput()
    {
        ColourSelectedButton();

        if (Input.GetAxis("HorizontalDPAD") > 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(1);
        }
        if (Input.GetAxis("HorizontalDPAD") < 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(-1);
        }
        if (Input.GetAxis("HorizontalDPAD") == 0f)
        {
            m_AxisInUse = false;
        }
        if (Input.GetButtonDown("Submit"))
        {
            m_Buttons[m_SelectedButtonPos].onClick.Invoke();
        }
    }

    private void ColourSelectedButton()
    {
        foreach (Button button in m_Buttons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
        m_Buttons[m_SelectedButtonPos].GetComponent<Image>().color = Color.yellow;
    }

    private void ChangeButtonChoice(int posChange)
    {
        m_SelectedButtonPos += posChange;

        if (m_SelectedButtonPos <= 0)
        {
            m_SelectedButtonPos = 0;
            return;
        }
        if (m_SelectedButtonPos > m_Buttons.Length - 1)
        {
            m_SelectedButtonPos = m_Buttons.Length - 1;
            return;
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        m_CharMenuRef.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        m_OptionsMenuRef.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
