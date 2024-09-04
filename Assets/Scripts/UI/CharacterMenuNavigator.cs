using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterMenuNavigator : Navigator2D
{
    protected override void HandleInput()
    {
        TextMeshProUGUI backText = m_BackButton?.GetComponentInChildren<TextMeshProUGUI>();

        if (Gamepad.current != null)
        {
            backText.GetComponent<RectTransform>().anchoredPosition = new Vector2(18, 0);

            foreach (Image button in m_ControllerButtons)
            {
                button.enabled = true;
            }
        }
        else
        {
            backText.transform.position = Vector2.zero;

            foreach (Image button in m_ControllerButtons)
            {
                button.enabled = false;
            }
        }

        HandleSelectionInput();

        if (Input.GetButtonDown("Submit"))
        {
            Button button = GetSelectableFromXY(m_SelectedButtonPosV2).GetComponent<Button>();
            if (button) button.onClick.Invoke();
        }
    }

    protected override void ChangeButtonChoice(int x, int y)
    {
        m_SelectedButtonPosV2.x += x;
        m_SelectedButtonPosV2.y += y;

        if (m_SelectedButtonPosV2.x <= 0)
        {
            m_SelectedButtonPosV2.x = 0;
        }
        if (m_SelectedButtonPosV2.y <= 0)
        {
            m_SelectedButtonPosV2.y = 0;
        }
        if (m_SelectedButtonPosV2.y > m_2DSelectables.Length - 1)
        {
            m_SelectedButtonPosV2.y = m_2DSelectables.Length - 1;
            m_SelectedButtonPosV2.x = 0;
        }
        if (m_SelectedButtonPosV2.x > m_2DSelectables[(int)m_SelectedButtonPosV2.y].row.Length - 1)
        {
            m_SelectedButtonPosV2.x = m_2DSelectables[(int)m_SelectedButtonPosV2.y].row.Length - 1;
        }

        if (y != 0) m_SelectedButtonPosV2.x = 0;

        if (m_Type == Type.Character) Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPosV2).transform);
    }
}
