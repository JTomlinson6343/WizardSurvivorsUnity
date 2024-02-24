using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public bool m_Unlocked;

    public Color m_Color;
    public Color m_CurrentColor;

    public string m_SkillName;

    [TextArea(3, 10)]
    public string[] m_Description;

    public string[] m_OnLevelUpDescription;

    public int[] m_Cost;

    public SkillData m_Data;

    [SerializeField] TextMeshProUGUI m_LevelIndicator;

    // List of skills that unlock this skill
    [SerializeField] protected SkillIcon[] m_Prerequisites;

    [SerializeField] Image[] m_PrevLines;
    [SerializeField] Image[] m_NextLines;

    private SkillTree m_SkillTreeRef;
    private Button m_ButtonRef;

    private void Start()
    {
        m_SkillTreeRef = GetComponentInParent<SkillTree>();
        m_ButtonRef = GetComponent<Button>();
        if (m_ButtonRef != null)
        {
            m_ButtonRef.onClick.AddListener(OnClick);
        }
    }

    public void InitFromFile(int level)
    {
        if (level <= 0) return;

        Unlock();
        m_Data.level = level;
        SetLevelIndicator();
    }

    public bool CheckPrerequisites()
    {
        if (m_Prerequisites.Length == 0)
        {
            return true;
        }

        bool canEnable = false;
        foreach (SkillIcon preReq in m_Prerequisites)
        {
            canEnable = canEnable || preReq.m_Unlocked;
        }
        return canEnable;
    }

    public bool IsMaxed()
    {
        return m_Data.level >= m_Data.maxLevel;
    }

    private void OnClick()
    {
        m_SkillTreeRef.SetHighlightedSkill(this);
    }

    public void Unlock()
    {
        m_Unlocked = true;
        ColourIn(m_Color);
    }

    private void ColourIn(Color color)
    {
        foreach (var item in GetComponentsInChildren<Image>())
        {
            item.color = color;
        }

        m_LevelIndicator.color = color;
        m_CurrentColor = color;
    }

    public void LevelUp()
    {
        m_Data.level++;
    }

    public void Lock()
    {
        m_Unlocked = false;
    }

    public void SetLevelIndicator()
    {
        m_LevelIndicator.text = m_Data.level.ToString() + "/" + m_Data.maxLevel.ToString();
    }

    // Displays skills as light or dark based on if theyre able to be unlocked
    public void ColorCheck()
    {
        LineColorCheck();
        if (m_Unlocked) return;

        if (CheckPrerequisites())
            ColourIn(Color.white);
        else
            ColourIn(Color.gray);
        SetLevelIndicator();
    }

    void LineColorCheck()
    {
        foreach (Image line in m_NextLines)
        {
            line.color = Color.gray;
        }

        if (!m_Unlocked) return;

        foreach (Image line in m_NextLines)
        {
            line.color = Color.white;
        }

        for (int i = 0; i < m_PrevLines.Length; i++)
        {
            for (int j = 0; j < m_Prerequisites.Length; j++)
            {
                for (int k = 0; k < m_Prerequisites[j].m_NextLines.Length; k++)
                {
                    if (m_PrevLines[i] == m_Prerequisites[j].m_NextLines[k] && m_Prerequisites[j].m_Unlocked)
                        m_PrevLines[i].color = m_Color;
                }
            }
        }
    }
}
