using UnityEngine;

public class HammerHit : MonoBehaviour
{
    public float minHitForce = 1.5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < minHitForce)
            return;

        BreakableWall wall =
            collision.gameObject.GetComponentInParent<BreakableWall>();

        if (wall != null)
        {
            wall.Hit();
        }
    }
}
