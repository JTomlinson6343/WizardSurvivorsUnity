using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSkillIcon : MonoBehaviour
{
    private SkillData m_Data;

    private Color m_Color; 

    public void Init(SkillData data)
    {
        m_Data = data;
        m_Color = PlayerManager.m_SkillTreeRef.m_CharacterColour;

        Image image = GetComponent<Image>();

        image.color = m_Color;

        if (!PlayerManager.m_SkillTreeRef.GetSkillIconWithID(m_Data.id)) return;

        image.sprite = PlayerManager.m_SkillTreeRef.GetSkillIconWithID(m_Data.id).GetComponent<Image>().sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCooldown(10f);
        }
    }
    public void StartCooldown(float cooldown)
    {
        StartCoroutine(StartCooldownAnim(cooldown));
    }

    private IEnumerator StartCooldownAnim(float cooldown)
    {
        Image image = GetComponent<Image>();
        image.color = Color.black;
        float alpha = 0f;

        while (true)
        {
            if (image.color == m_Color) break;

            alpha += 1f / cooldown;

            image.color = Color.Lerp(Color.grey, m_Color, alpha * 0.2f);

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }
}