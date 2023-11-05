using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Ability
{
    [SerializeField] GameObject m_FlamethrowerObject;

    private void Start()
    {
        m_FlamethrowerObject.transform.SetParent(Player.m_Instance.GetStaffTransform(), false);
    }
    private void Update()
    {
        Vector2 vec = Player.m_Instance.GetAimDirection().normalized;
        m_FlamethrowerObject.SetActive(Input.GetMouseButton(0));
        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90);
    }
}
