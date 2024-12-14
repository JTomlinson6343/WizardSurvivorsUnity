using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDSkillIcon : MonoBehaviour
{
    private SkillData m_Data;

    private Color m_Color;

    [SerializeField] TextMeshProUGUI m_SecondsIndicator;

    public void Init(SkillData data)
    {
        m_Data = data;
        m_Color = PlayerManager.m_SkillTreeRef.m_CharacterColour;

        Image image = GetComponent<Image>();

        image.color = m_Color;

        if (!PlayerManager.m_SkillTreeRef.GetSkillIconWithID(m_Data.id)) return;

        image.sprite = PlayerManager.m_SkillTreeRef.GetSkillIconWithID(m_Data.id).GetComponent<Image>().sprite;
    }

    public void StartCooldown(float cooldown)
    {
        Image image = GetComponent<Image>();
        // Reset color to black
        image.color = Color.black;

        // Start the LeanTween animation
        LeanTween.value(gameObject, 0f, 1f, cooldown)
            .setOnUpdate((float alpha) =>
            {
                // Lerp color from black to white
                image.color = Color.Lerp(Color.black, Color.white, alpha);

                // Update remaining time
                float timeLeft = Mathf.Lerp(cooldown, 0f, alpha);
                SetText(timeLeft);
            })
            .setOnComplete(() =>
            {
                // Set to final color after completion
                image.color = m_Color;
            });
    }

    private void SetText(float timeLeft)
    {
        // Display time left on icon
        if (timeLeft > 0) m_SecondsIndicator.text = ((int)timeLeft).ToString();
        else m_SecondsIndicator.text = "";
    }
}