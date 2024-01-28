using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSkillIcon : MonoBehaviour
{
    private SkillData m_Data;

    private float m_StartScale = 1f;

    public void Init(SkillData data)
    {
        m_Data = data;
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
            if (image.color == Color.white) break;

            alpha += 1f / cooldown;

            image.color = Color.Lerp(Color.black, m_Data.color, alpha * 0.2f);

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }
}