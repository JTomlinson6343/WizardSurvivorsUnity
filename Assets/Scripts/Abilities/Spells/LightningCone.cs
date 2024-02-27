using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCone : Spell
{
    [SerializeField] GameObject m_LightningCone;
    [SerializeField] float m_Lifetime;
    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = GameplayManager.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            ResetCooldown(kCooldownAfterReset);
            return;
        }

        Vector2 dir = GameplayManager.GetDirectionToGameObject(Player.m_Instance.GetStaffTransform().position, closestEnemy);
    }

    public override void OnMouseInput(Vector2 aimDirection)
    {
        GameObject cone = Instantiate(m_LightningCone);

        cone.GetComponent<AOEObject>().StartLifetimeTimer(m_Lifetime);
        cone.GetComponent<AOEObject>().m_AbilitySource = this;
        cone.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90);
        cone.transform.SetParent(Player.m_Instance.GetStaffTransform());
        cone.transform.position = Player.m_Instance.GetStaffTransform().position;
    }
}
