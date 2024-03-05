using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    private enum NavDirection
    {
        Horizontal,
        Vertical
    }

    [SerializeField] Selectable[] m_Selectables;

    [SerializeField] NavDirection m_Direction = NavDirection.Horizontal;

    [SerializeField] bool m_LabelSelectEnabled;

    [SerializeField] TextMeshProUGUI[] m_Labels;

    int m_SelectedButtonPos;
    bool m_AxisInUse;

    // Use this for initialization
    void Start()
    {
        m_SelectedButtonPos = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Gamepad.current != null)
            HandleInput();
    }

    protected void HandleInput()
    {
        ColourSelectedButton();

        HandleSelectionInput();

        if (Input.GetButtonDown("Submit"))
        {
            Button button = m_Selectables[m_SelectedButtonPos].GetComponent<Button>();

            if (button) button.onClick.Invoke();

            Toggle toggle = m_Selectables[m_SelectedButtonPos].GetComponent<Toggle>();

            if (toggle) HandleToggleInput(toggle);
        }
        Scrollbar scrollbar = m_Selectables[m_SelectedButtonPos].GetComponent<Scrollbar>();

        if (scrollbar) HandleScrollbarInput(scrollbar);
    }

    private void HandleSelectionInput()
    {
        string axis;

        if (m_Direction == NavDirection.Vertical)
        {
            axis = "VerticalDPAD";
        }
        else
        {
            axis = "HorizontalDPAD";
        }

        if (Input.GetAxis(axis) > 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(1);
        }
        if (Input.GetAxis(axis) < 0f && m_AxisInUse == false)
        {
            m_AxisInUse = true;
            ChangeButtonChoice(-1);
        }
        if (Input.GetAxis(axis) == 0f)
        {
            m_AxisInUse = false;
        }
    }

    private void ColourSelectedButton()
    {
        if (m_LabelSelectEnabled)
        {
            foreach (TextMeshProUGUI label in m_Labels)
            {
                label.color = Color.white;
            }
            m_Labels[m_SelectedButtonPos].color = Color.yellow;
            return;
        }

        foreach (Selectable selectable in m_Selectables)
        {
            selectable.GetComponent<Image>().color = Color.white;
        }
        m_Selectables[m_SelectedButtonPos].GetComponent<Image>().color = Color.yellow;
    }

    private void HandleScrollbarInput(Scrollbar scrollbar)
    {
        if (Input.GetAxis("HorizontalDPAD") < 0f)
        {
            scrollbar.value -= 0.05f;
        }
        if (Input.GetAxis("HorizontalDPAD") > 0f)
        {
            scrollbar.value += 0.05f;
        }
    }

    private void HandleToggleInput(Toggle toggle)
    {
        if (toggle.isOn)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }

    private void ChangeButtonChoice(int posChange)
    {
        if (m_Direction == NavDirection.Vertical) posChange *= -1;

        m_SelectedButtonPos += posChange;

        if (m_SelectedButtonPos <= 0)
        {
            m_SelectedButtonPos = 0;
            return;
        }
        if (m_SelectedButtonPos > m_Selectables.Length - 1)
        {
            m_SelectedButtonPos = m_Selectables.Length - 1;
            return;
        }
    }
}
