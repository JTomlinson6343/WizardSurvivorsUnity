using System.Collections;
using UnityEngine;

public class HitParticles : MonoBehaviour
{
    public void FaceDirection(Vector2 dir)
    {
        Utils.PointInDirection(dir, gameObject);
    }
}