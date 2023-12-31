using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionEnemy : Enemy
{
    void UpdateHealthBar()
    {
        Actor actorComponent = gameObject.GetComponentInChildren<Actor>();

        float health = actorComponent.GetHealthAsRatio();

        BasicBar bar = gameObject.GetComponentInChildren<BasicBar>();
    }

    public override void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING) { return; }

        base.Update();
        UpdateHealthBar();
    }
}