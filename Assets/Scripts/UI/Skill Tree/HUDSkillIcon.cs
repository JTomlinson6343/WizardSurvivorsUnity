using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDSkillIcon : MonoBehaviour
{
    private SkillData m_Data;

    private Color m_Color;

    private readonly float kCooldownAnimationInterval = 0.5f;

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
        StopAllCoroutines();
        StartCoroutine(StartCooldownAnim(cooldown));
    }

    private IEnumerator StartCooldownAnim(float cooldown)
    {
        Image image = GetComponent<Image>();
        image.color = Color.black;
        float alpha = 0f;

        float timeLeft = cooldown;

        SetText(timeLeft);
        while (timeLeft > 0f)
        {
            yield return new WaitForSeconds(kCooldownAnimationInterval);
            // Increment alpha for the lerp
            alpha += 1f / cooldown;

            // Lerp between black and white
            image.color = Color.Lerp(Color.black, Color.white, alpha * kCooldownAnimationInterval);

            // Decrease time over time
            timeLeft -= kCooldownAnimationInterval;

            SetText(timeLeft);
        }

        // Set colour to the character's colour
        image.color = m_Color;

        yield return null;
    }

    private void SetText(float timeLeft)
    {
        // Display time left on icon
        if (timeLeft > 0) m_SecondsIndicator.text = ((int)timeLeft).ToString();
        else m_SecondsIndicator.text = "";
    }
}