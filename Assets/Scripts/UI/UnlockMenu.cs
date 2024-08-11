using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockMenu : MonoBehaviour
{
    [SerializeField] MainMenu m_MainMenuRef;

    [SerializeField] RectTransform m_UnlockPanel;
    [SerializeField] GameObject m_UnlockPanelRowPrefab;
    [SerializeField] Scrollbar m_Scrollbar;

    private void Awake()
    {
        PopulateUnlocksList();
    }
    public void Open(GameObject prevMenu)
    {
        prevMenu.SetActive(false);
        gameObject.SetActive(true);
        GetComponent<Navigator>().Start();
        UpdateRowValues();
        m_Scrollbar.value = 0;
        m_Scrollbar.gameObject.SetActive(m_UnlockPanel.transform.childCount > 3);
    }

    void PopulateUnlocksList()
    {
        List<UnlockCondition> unlocks = UnlockManager.GetUnlockConditions();

        foreach (UnlockCondition condition in unlocks)
        {
            GameObject row = Instantiate(m_UnlockPanelRowPrefab);
            row.GetComponent<UnlockPanelRow>().Init(condition);

            row.transform.SetParent(m_UnlockPanel.transform, false);
        }
    }

    void UpdateRowValues()
    {
        foreach (UnlockPanelRow row in GetComponentsInChildren<UnlockPanelRow>())
        {
            row.UpdateValues();
        }
    }

    public void Scroll(float value)
    {
        const float rowHeight = 130f;
        const float spacing = 5f;
        int length = m_UnlockPanel.transform.childCount - 3;

        m_UnlockPanel.anchoredPosition = new Vector2(0, value * (rowHeight + spacing) * length);
    }

    public void OnBackPressed()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
        m_MainMenuRef.GetComponent<Navigator>().Start();
    }
}
