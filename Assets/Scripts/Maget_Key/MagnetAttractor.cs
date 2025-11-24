using UnityEngine;

public class MagnetAttractor : MonoBehaviour
{
    public Transform magnetHoldPoint;
    public float attractDistance = 0.4f;
    public float attractSpeed = 5f;

    [HideInInspector]
    public bool isActive = false; 

    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;

        if (!other.CompareTag("Key")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 target = magnetHoldPoint.position;
        Vector3 dir = target - rb.position;
        if (dir.magnitude > attractDistance)
        {
            Vector3 newPos = Vector3.MoveTowards(
                rb.position,
                target,
                attractSpeed * Time.deltaTime
            );

            rb.MovePosition(newPos);
        }
    }
}
