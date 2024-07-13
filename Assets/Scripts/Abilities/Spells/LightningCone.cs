using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightningCone : Spell
{
    [SerializeField] GameObject m_LightningCone;
    [SerializeField] float m_Lifetime;
    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        Vector2 dir = Utils.GetDirectionToGameObject(Player.m_Instance.GetStaffTransform().position, closestEnemy);

        ShootCone(dir);
    }

    public override void OnMouseInput(Vector2 aimDirection)
    {
        ShootCone(aimDirection);
    }

    private void ShootCone(Vector2 dir)
    {
        GameObject cone = Instantiate(m_LightningCone);

        cone.GetComponent<AOEObject>().StartLifetimeTimer(m_Lifetime);
        cone.GetComponent<AOEObject>().m_AbilitySource = this;
        cone.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
        cone.transform.SetParent(Player.m_Instance.transform);
        cone.transform.position = Player.m_Instance.GetStaffTransform().position;

        AudioManager.m_Instance.PlaySound(24, 0.3f);
    }
}
