using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public static SkillPanel m_Instance;

    [SerializeField] GameObject m_SkillIconPrefab;

    // Icon arrange constants
    [SerializeField] float m_ColumnSpacing;
    [SerializeField] Vector2 m_IconSize;
    [SerializeField] float m_RowSpacing;
    [SerializeField] int   m_RowSize;

    private void Awake()
    {
        m_Instance = this;
    }

    public void Init()
    {
        if (!InitHUDSkills()) return;

        ArrangeIcons(GetComponentsInChildren<HUDSkillIcon>());
    }

    private void ArrangeIcons(HUDSkillIcon[] icons)
    {
        int column = 0;
        int row = 0;
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(column * (m_ColumnSpacing + m_IconSize.x), -row * (m_RowSpacing + m_IconSize.y));

            column++;
            if (column >= m_RowSize)
            {
                row++;
                column = 0;
            }
        }
    }

    private bool InitHUDSkills()
    {
        if (SkillManager.m_Instance.GetCooldownSkills().Length == 0) return false;

        foreach (CooldownSkill skill in SkillManager.m_Instance.GetCooldownSkills())
        {
            GameObject skillIcon = Instantiate(m_SkillIconPrefab);

            HUDSkillIcon icon = skillIcon.GetComponent<HUDSkillIcon>();

            skill.InitHUDSkillIcon(icon);

            skillIcon.transform.SetParent(transform, false);
        }

        return true;
    }
}
