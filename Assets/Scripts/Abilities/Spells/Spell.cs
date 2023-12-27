using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Ability
{
    public override void OnChosen()
    {
        base.OnChosen();
        m_isMaxed = true;
    }
}
