using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPanelRow : MonoBehaviour
{
    [SerializeField] Image m_Icon;
    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;

    private UnlockCondition m_Condition;

    public void Init(UnlockCondition unlockCondition)
    {
        m_Condition = unlockCondition;
        UpdateValues();
    }

    public void UpdateValues()
    {
        m_Icon.sprite = m_Condition.image;
        m_NameLabel.text = m_Condition.name;

        if (m_Condition.doFormatMessage) m_DescriptionLabel.text = m_Condition.FormatConditionMessage();
        else m_DescriptionLabel.text = m_Condition.message;

        if (!UnlockManager.GetUnlockableWithName(m_Condition.name).unlocked) m_Icon.color = Color.grey;
        else m_Icon.color = Color.white;
    }
}
