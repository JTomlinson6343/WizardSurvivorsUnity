using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Ability
{
    [SerializeField] GameObject m_FlamethrowerObject;
    [HideInInspector] public Firebolt m_FireballRef;

    private float m_LastCast;
    private float m_LockoutTime = 0.5f;

    private bool m_Autofire = false;

    override public void Start()
    {
        base.Start();

        m_FlamethrowerObject.GetComponentInChildren<ConstantDamageAOE>().m_AbilitySource = this;
    }
    private void Update()
    {
        if (StateManager.IsGameplayStopped()) { return; }

        if (!m_Enabled) return;

        if (m_Autofire)
        {
            AutoCast();
            return;
        }
        if (!(MouseInput() || ControllerInput()))
        {
            m_FlamethrowerObject.SetActive(false);
        }

        transform.position = Player.m_Instance.GetStaffTransform().position;
    }

    private bool MouseInput()
    {
        if (!Input.GetMouseButton(0)) return false;
        
        // While left mouse is held, enable flamethrower after a short lockout delay
        Vector2 vec = Player.m_Instance.GetMouseAimDirection().normalized;
        
        float now = Time.realtimeSinceStartup;

        if (now - m_LastCast > m_LockoutTime)
        {
            m_FlamethrowerObject.SetActive(true);
            m_LastCast = now;
        }
        
        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90);

        return true;
    }

    public override void ToggleAutofire(bool on)
    {
        m_Autofire = on;
    }

    private void AutoCast()
    {
        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetCentrePos(), m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            m_FlamethrowerObject.SetActive(false);
            return;
        }
        Vector2 dir = Utils.GetDirectionToGameObject(Player.m_Instance.GetStaffTransform().position, closestEnemy);

        float now = Time.realtimeSinceStartup;

        if (now - m_LastCast > m_LockoutTime)
        {
            m_FlamethrowerObject.SetActive(true);
            m_LastCast = now;
        }

        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

        transform.position = Player.m_Instance.GetStaffTransform().position;
    }

    private bool ControllerInput()
    {
        if (!(Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)) return false;

        // While left mouse is held, enable flamethrower after a short lockout delay
        Vector2 vec = Player.m_Instance.GetControllerAimDirection();

        float now = Time.realtimeSinceStartup;

        if (now - m_LastCast > m_LockoutTime)
        {
            m_FlamethrowerObject.SetActive(true);
            m_LastCast = now;
        }

        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90);

        return true;
    }

    public void Enable()
    {
        m_Enabled = true;
    }
}
