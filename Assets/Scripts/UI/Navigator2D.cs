using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Navigator2D : Navigator
{
    [System.Serializable]
    protected struct Selectable2DArray{
        public Selectable[] row;
    }

    [SerializeField] protected Selectable2DArray[] m_2DSelectables;
    protected Vector2 m_SelectedButtonPosV2;
    [SerializeField] Button m_UnlockButton;
    [SerializeField] Button m_RespecButton;
    [SerializeField] Vector2Int m_DefaultSelectableIndex = new Vector2Int(0,0);
    [SerializeField] bool m_InvokeOnSelect = true;

    public override void Start()
    {
        m_SelectedButtonPosV2 = m_DefaultSelectableIndex;
        Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPosV2).transform);
        if(m_InvokeOnSelect) GetSelectableFromXY(m_SelectedButtonPosV2).GetComponent<Button>().onClick.Invoke();
    }

    protected override void HandleInput()
    {
        TextMeshProUGUI unlockText = m_UnlockButton?.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI respecText = m_RespecButton?.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI backText = m_BackButton?.GetComponentInChildren<TextMeshProUGUI>();

        if (Gamepad.current != null)
        {
            if (unlockText) unlockText.GetComponent<RectTransform>().anchoredPosition = new Vector2(18, 0);
            if (respecText) respecText.GetComponent<RectTransform>().anchoredPosition = new Vector2(18, 0);
            if (backText)   backText.GetComponent<RectTransform>().anchoredPosition = new Vector2(18, 0);

            foreach (Image button in m_ControllerButtons)
            {
                button.enabled = true;
            }
        }
        else
        {
            if (unlockText) unlockText.transform.position = Vector2.zero;
            if (respecText) respecText.transform.position = Vector2.zero;
            if (backText)   backText.transform.position = Vector2.zero;

            foreach (Image button in m_ControllerButtons)
            {
                button.enabled = false;
            }
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
    virtual protected void ChangeButtonChoice(int x, int y)
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
        }
        if (m_SelectedButtonPosV2.x > m_2DSelectables[(int)m_SelectedButtonPosV2.y].row.Length - 1)
        {
            m_SelectedButtonPosV2.x = m_2DSelectables[(int)m_SelectedButtonPosV2.y].row.Length - 1;
        }
        if (!m_InvokeOnSelect) return;

        Button button = GetSelectableFromXY(m_SelectedButtonPosV2).GetComponent<Button>();

        if (button) button.onClick.Invoke();
    }

    protected Selectable GetSelectableFromXY(int x, int y)
    {
        return m_2DSelectables[y].row[x];
    }

    protected Selectable GetSelectableFromXY(Vector2 xy)
    {
        return m_2DSelectables[(int)xy.y].row[(int)xy.x];
    }
}
