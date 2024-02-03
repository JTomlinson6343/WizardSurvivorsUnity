using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementalSpell : Spell
{
    [SerializeField] GameObject m_FireElementalPrefab;

    [SerializeField] GameObject m_SmokePrefab;
    public override void OnCast()
    {
        base.OnCast();

        GameObject fireElemental = Instantiate(m_FireElementalPrefab);
        GameObject smoke = Instantiate(m_SmokePrefab);

        fireElemental.transform.position = Player.m_Instance.transform.position;
        smoke.transform.position = fireElemental.transform.position;

        smoke.transform.localScale = fireElemental.transform.localScale * 2f;

        fireElemental.GetComponent<FireElemental>().m_AbilitySource = this;
    }
}
