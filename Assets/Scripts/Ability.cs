using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public struct AbilityStats
{
    public float aoe;           // Modifier of radius
    public float duration;      // Durarion in seconds
    public float damageScaling; // Percentage of player damage dealt by the ability
    public float speed;         // Speed of projectile/animation of the ability
    public float cooldown;      // Cooldown in seconds of the ability
    public float amount;        // Amount of projectiles fired by the ability
    public float knockback;     // Knockback of the ability
    public int   pierceAmount;  // Number of enemies that can be pierced
}

public class Ability : MonoBehaviour
{
    // Level of the ability
    int m_Level;

    AbilityStats m_BaseStats;   // Base stats of the ability
    AbilityStats m_BonusStats;  // Bonus stats gained when ability is leveled up
    AbilityStats m_TotalStats;  // Total combined stats combining base stats, bonus stats and character stats (from buffs)

    void LevelUp()
    {
        switch (m_Level)
        {
            case 1:
                Level2();
                break;
            case 2:
                Level3();
                break;
            case 3:
                Level4();
                break;
            case 4:
                Level5();
                break;
            default:
                break;
        }
    }

    // Functions called when the ability is upgraded to the specific level
    protected virtual void Level2()
    {

    }
    protected virtual void Level3()
    {

    }
    protected virtual void Level4()
    {

    }
    protected virtual void Level5()
    {

    }
}
