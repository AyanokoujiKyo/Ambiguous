using UnityEngine;

public class HammerHit : MonoBehaviour
{
    [Header("Hit filtering")]
    public float minHitSpeed = 1.0f;     
    public float maxHitSpeed = 8.0f;

    [Header("Release & push")]
    public float baseImpulse = 2.0f;    
    public float impulsePerSpeed = 1.5f; 
    public float releaseRadius = 0.0f;  

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        if (speed < minHitSpeed) return;

        var freeze = collision.collider.GetComponentInParent<WallFreezeController>();
        if (freeze == null || !freeze.IsUnlocked())
            return; 
        Rigidbody rb = collision.rigidbody;
        if (rb == null)
            rb = collision.collider.GetComponentInParent<Rigidbody>();

        if (rb == null)
            return; 
        Vector3 contact = collision.GetContact(0).point;
        Vector3 dir = (contact - transform.position);
        if (dir.sqrMagnitude < 0.0001f) dir = collision.relativeVelocity;
        dir = dir.normalized;

        speed = Mathf.Clamp(speed, minHitSpeed, maxHitSpeed);

        float impulse = baseImpulse + speed * impulsePerSpeed;
        freeze.ReleaseChunkAndPush(rb, dir * impulse);
        if (releaseRadius > 0.01f)
        {
            freeze.ReleaseAround(contact, releaseRadius, dir * (impulse * 0.6f));
        }
    }
}
