using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Ability
{
    [SerializeField] GameObject m_FlamethrowerObject;
    [HideInInspector] public Firebolt m_FireballRef;

    private float m_LastCast;
    private float m_LockoutTime = 0.5f;

    override public void Start()
    {
        base.Start();

        m_FlamethrowerObject.transform.SetParent(Player.m_Instance.GetStaffTransform(), false);
        m_FlamethrowerObject.GetComponentInChildren<DebuffAOE>().m_AbilitySource = this;
    }
    private void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING && StateManager.GetCurrentState() != State.BOSS) { return; }

        if (!m_Enabled) return;

        // While left mouse is held, enable flamethrower after a short lockout delay
        Vector2 vec = Player.m_Instance.GetAimDirection().normalized;
        if (Input.GetMouseButton(0))
        {
            float now = Time.realtimeSinceStartup;

            if (now - m_LastCast > m_LockoutTime)
            {
                m_FlamethrowerObject.SetActive(true);
                m_LastCast = now;
            }
        }
        else
        {
            m_FlamethrowerObject.SetActive(false);
        }
        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90);
    }

    public void Enable()
    {
        m_Enabled = true;
    }
}
