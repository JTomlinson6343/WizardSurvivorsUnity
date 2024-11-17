using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterMenuNavigator : Navigator2D
{
    [SerializeField] float[] m_ResetXRows;
    bool firstTimeOpened = true;
    public override void Start()
    {
        if (Gamepad.current == null) return;

        m_SelectedButtonPosV2 = m_DefaultSelectableIndex;
        Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPosV2).transform);
        if (m_InvokeOnSelect) GetSelectableFromXY(m_SelectedButtonPosV2).GetComponent<Button>().onClick.Invoke();
    }
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
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_BackButton) m_BackButton.onClick.Invoke();
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

        if (m_ResetXRows.Contains(m_SelectedButtonPosV2.y) && x == 0) m_SelectedButtonPosV2.x = 0;

        if (x != 0) m_SelectedButtonPosV2 = TrySkipInvalidSelectable(m_SelectedButtonPosV2, x, y);

        if (m_Type == Type.Character) Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPosV2).transform);
    }

    Vector2 TrySkipInvalidSelectable(Vector2 pos, float xIncrement, float yIncrement)
    {
        Selectable selectable = GetSelectableFromXY(pos);
        if (selectable)
        {
            if (!selectable.gameObject.activeSelf || !selectable.interactable)
            {
                pos.x += xIncrement;
                return TrySkipInvalidSelectable(pos, xIncrement, yIncrement);
            }
            else
            {
                return pos;
            }
        }
        pos.x -= xIncrement;
        pos.y -= yIncrement;
        return pos;
    }
}
