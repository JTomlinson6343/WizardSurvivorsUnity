using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Navigator2D : Navigator
{
    const string unlockStr = "Unlock!";
    const string respecStr = "Respec";

    [System.Serializable]
    private struct Selectable2DArray{
        public Selectable[] row;
    }

    [SerializeField] Selectable2DArray[] m_2DSelectables;
    Vector2 m_SelectedButtonPos;
    [SerializeField] Button m_UnlockButton;
    [SerializeField] Button m_RespecButton;

    public override void Start()
    {
        m_SelectedButtonPos = Vector2.zero;
        Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPos).transform);
        GetSelectableFromXY(m_SelectedButtonPos).GetComponent<Button>().onClick.Invoke();
    }

    protected override void HandleInput()
    {
        if (Gamepad.current != null)
        {
            m_UnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "(A) "+ unlockStr;
            m_RespecButton.GetComponentInChildren<TextMeshProUGUI>().text = "(Y) "+ respecStr;
        }
        else
        {
            m_UnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = unlockStr;
            m_RespecButton.GetComponentInChildren<TextMeshProUGUI>().text = respecStr;
        }

        HandleSelectionInput();

        if (Input.GetButtonDown("Submit"))
        {
            if (m_UnlockButton && m_UnlockButton.interactable && m_UnlockButton.isActiveAndEnabled) 
                m_UnlockButton.onClick.Invoke();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_BackButton) m_BackButton.onClick.Invoke();
        }
        if (Input.GetButtonDown("Respec"))
        {
            if (m_RespecButton) m_RespecButton.onClick.Invoke();
        }
    }

    protected override void HandleSelectionInput()
    {
        if (Input.GetAxis("VerticalDPAD") > 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(0, 1);
        }
        if (Input.GetAxis("VerticalDPAD") < 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(0, -1);
        }
        if (Input.GetAxis("HorizontalDPAD") > 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(1, 0);
        }
        if (Input.GetAxis("HorizontalDPAD") < 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(-1, 0);
        }

        if (Input.GetAxis("HorizontalDPAD") == 0f && Input.GetAxis("VerticalDPAD") == 0)
        {
            m_AxisInUse = false;
        }
    }

    // Change in x or y
    private void ChangeButtonChoice(int x, int y)
    {
        m_SelectedButtonPos.x += x;
        m_SelectedButtonPos.y += y;

        if (m_SelectedButtonPos.x <= 0)
        {
            m_SelectedButtonPos.x = 0;
        }
        if (m_SelectedButtonPos.y <= 0)
        {
            m_SelectedButtonPos.y = 0;
        }
        if (m_SelectedButtonPos.y > m_2DSelectables.Length - 1)
        {
            m_SelectedButtonPos.y = m_2DSelectables.Length - 1;
        }
        if (m_SelectedButtonPos.x > m_2DSelectables[(int)m_SelectedButtonPos.y].row.Length - 1)
        {
            m_SelectedButtonPos.x = m_2DSelectables[(int)m_SelectedButtonPos.y].row.Length - 1;
        }

        Button button = GetSelectableFromXY(m_SelectedButtonPos).GetComponent<Button>();

        if (button) button.onClick.Invoke();
    }

    Selectable GetSelectableFromXY(int x, int y)
    {
        return m_2DSelectables[y].row[x];
    }

    Selectable GetSelectableFromXY(Vector2 xy)
    {
        return m_2DSelectables[(int)xy.y].row[(int)xy.x];
    }
}
