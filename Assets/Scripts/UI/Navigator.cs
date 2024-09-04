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
    protected enum Type
    {
        Options,
        Character
    }

    [SerializeField] Selectable[] m_Selectables;
    [SerializeField] protected Button m_BackButton;

    [SerializeField] bool m_RememberSelectedButtonPos = true;
    [SerializeField] NavDirection m_Direction = NavDirection.Horizontal;
    [SerializeField] NavDirection m_ScrollbarDirection = NavDirection.Horizontal;
    [SerializeField] protected Type m_Type = Type.Character;

    [SerializeField] bool m_LabelSelectEnabled;
    [SerializeField] bool m_HighlightTextEnabled;

    [SerializeField] TextMeshProUGUI[] m_Labels;

    [SerializeField] protected Image[] m_ControllerButtons;

    int m_SelectedButtonPos = 0;
    protected bool m_AxisInUse;

    float m_LastScrollbar;
    readonly float m_ScrollbarDelay = 0.02f;

    // Use this for initialization
    public virtual void Start()
    {
        if (!m_RememberSelectedButtonPos) m_SelectedButtonPos = 0;
        if (m_Selectables.Length >0 && m_Type == Type.Character && Gamepad.current != null) Utils.SetSelectedAnimTarget(m_Selectables[m_SelectedButtonPos].transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Gamepad.current != null)
        {
            HandleInput();
            if (m_ControllerButtons.Length < 1) return;
            foreach (Image button in m_ControllerButtons)
            {
                button.gameObject.SetActive(true);
                Button parentText = button.GetComponentInParent<Button>();
                if (parentText) parentText.GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>().anchoredPosition = new Vector2(18, 0);
            }
        }
        else
        {
            if (m_ControllerButtons.Length < 1) return;

            foreach (Image button in m_ControllerButtons)
            {
                button.gameObject.SetActive(false);
                Button parentText = button.GetComponentInParent<Button>();
                if (parentText) parentText.GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }

    protected virtual void HandleInput()
    {
        if (m_Selectables.Length == 0) return;

        ColourSelectedButton();
        HandleSelectionInput();

        if (Input.GetButtonDown("Submit"))
        {
            Button button = m_Selectables[m_SelectedButtonPos].GetComponent<Button>();

            if (button) button.onClick.Invoke();

            Toggle toggle = m_Selectables[m_SelectedButtonPos].GetComponent<Toggle>();

            if (toggle) HandleToggleInput(toggle);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_BackButton) m_BackButton.onClick.Invoke();
        }

        Scrollbar scrollbar = m_Selectables[m_SelectedButtonPos].GetComponent<Scrollbar>();

        if (scrollbar) HandleScrollbarInput(scrollbar);
    }

    protected virtual void HandleSelectionInput()
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
            if (selectable.GetComponentInChildren<TextMeshProUGUI>() && m_HighlightTextEnabled)
            {
                selectable.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                continue;
            }
        }

        if (m_Selectables[m_SelectedButtonPos].GetComponentInChildren<TextMeshProUGUI>() && m_HighlightTextEnabled)
        {
            m_Selectables[m_SelectedButtonPos].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            return;
        }
    }

    private void HandleScrollbarInput(Scrollbar scrollbar)
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastScrollbar < m_ScrollbarDelay) return;

        m_LastScrollbar = now;

        string dpadDir;
        int modifier = 1;
        float change = 0;

        if (m_ScrollbarDirection == NavDirection.Horizontal)
        {
            dpadDir = "HorizontalDPAD";
        }
        else
        {
            dpadDir = "VerticalDPAD";
            // Reverse direction if scrollbar is vertical
            modifier = -1;
        }

        if (Input.GetAxis(dpadDir) < 0f) change = -0.1f;
        
        if (Input.GetAxis(dpadDir) > 0f) change = 0.1f;
        

        scrollbar.value = Mathf.Clamp01(scrollbar.value + change * modifier);
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
        }
        else if (m_SelectedButtonPos > m_Selectables.Length - 1)
        {
            m_SelectedButtonPos = m_Selectables.Length - 1;
        }

        if (m_Type == Type.Character) Utils.SetSelectedAnimTarget(m_Selectables[m_SelectedButtonPos].transform);
    }
}
