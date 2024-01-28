using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public static SkillPanel m_Instance;

    private GameObject m_SkillIconPrefab;

    // Icon arrange constants
    [SerializeField] float m_ColumnSpacing;
    [SerializeField] Vector2 m_IconSize;
    [SerializeField] float m_RowSpacing;
    [SerializeField] int   m_RowSize;

    [SerializeField] SkillIcon[] m_Icons;

    private void Awake()
    {
        m_Instance = this;
    }

    public void Init(SkillTree skillTree)
    {

    }

    //private void ArrangeIcons(SkillIcon[] icons)
    //{
    //    int column = 0;
    //    int row = 0;
    //    for (int i = 0; i < icons.Length; i++)
    //    {
    //        icons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(column * (m_ColumnSpacing + m_IconSize.x), -row * (m_RowSpacing + m_IconSize.y));

    //        column++;
    //        if (column >= m_RowSize)
    //        {
    //            row++;
    //            column = 0;
    //        }
    //    }
    //}

    //private void InitHUDSkills()
    //{
    //    if (SkillManager.m_Instance.GetCooldownSkills().Length == 0) return;

    //    foreach (CooldownSkill skill in SkillManager.m_Instance.GetCooldownSkills())
    //    {
    //        GameObject skillIcon = Instantiate(m_SkillIconPrefab);

    //        skillIcon.GetComponent<HUDSkillIcon>();
    //    }
    //}
}
