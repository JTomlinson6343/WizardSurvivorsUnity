using System.Collections;
using UnityEngine;

public class Priest : Player
{
    protected override void FaceDirection(Vector2 dir)
    {
        base.FaceDirection(dir);

        // If velocity > 0, don't flip. if it is less than, flip
        float faceDir = dir.x > 0 ? 1f : -1f;

        m_Animator.speed = faceDir * m_Animator.speed;
        m_AnimatorMask.speed = faceDir * m_AnimatorMask.speed;
    }
}