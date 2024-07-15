using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator2D : Navigator
{
    [System.Serializable]
    private struct Selectable2DArray{
        public Selectable[] row;
    }

    [SerializeField] Selectable2DArray[] m_2DSelectables;
    Vector2 m_SelectedButtonPos;

    public override void Start()
    {
        m_SelectedButtonPos = Vector2.zero;
        Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPos).transform);
    }

    protected override void HandleInput()
    {
        HandleSelectionInput();

        if (Input.GetButtonDown("Submit"))
        {
            Button button = GetSelectableFromXY(m_SelectedButtonPos).GetComponent<Button>();

            if (button) button.onClick.Invoke();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_BackButton) m_BackButton.onClick.Invoke();
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

        Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPos).transform);
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
