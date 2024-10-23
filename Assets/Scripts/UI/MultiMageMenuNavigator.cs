using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MultiMageMenuNavigator : CharacterMenuNavigator
{
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

        if (!GetSelectableFromXY(m_SelectedButtonPosV2).gameObject.activeSelf
            || !GetSelectableFromXY(m_SelectedButtonPosV2).interactable)
        {
            m_SelectedButtonPosV2.x -= x;
            m_SelectedButtonPosV2.y -= y;
            return;
        }

        if (m_Type == Type.Character) Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPosV2).transform);
    }
}
