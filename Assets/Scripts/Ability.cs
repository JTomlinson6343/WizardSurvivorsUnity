using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public struct AbilityStats
{
    float damage;
    float aoe;
    float duration;
    float speed;
    float cooldown;
    float amount;
    float knockback;
    int   pierceAmount;
}

public class Ability : MonoBehaviour
{
    AbilityStats m_BaseStats;
    AbilityStats m_BonusStats;
}
