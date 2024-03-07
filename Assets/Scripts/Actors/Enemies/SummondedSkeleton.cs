using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedSkeleton : Skeleton
{
    override public void Update()
    {
        if (StateManager.GetCurrentState() != StateManager.State.PLAYING && StateManager.GetCurrentState() != StateManager.State.BOSS)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            return;
        }

        FollowPlayer();
    }
}
