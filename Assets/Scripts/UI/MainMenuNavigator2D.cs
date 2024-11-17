using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuNavigator2D : Navigator2D
{
    protected override void HandleInput()
    {
        HandleSelectionInput();

        if (Input.GetButtonDown("Submit"))
        {
            Button button = GetSelectableFromXY(m_SelectedButtonPosV2).GetComponent<Button>();
            if (button) button.onClick.Invoke();
        }
    }

    protected override void ChangeButtonChoice(int x, int y)
    {
        base.ChangeButtonChoice(x, y);
        if (m_Type == Type.Character) Utils.SetSelectedAnimTarget(GetSelectableFromXY(m_SelectedButtonPosV2).transform);
    }
}
