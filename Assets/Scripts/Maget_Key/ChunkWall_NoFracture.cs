using UnityEngine;

public class WallArmController : MonoBehaviour
{
    [Header("Wall chunks")]
    private Rigidbody[] chunkBodies;

    private void Awake()
    {
        chunkBodies = GetComponentsInChildren<Rigidbody>(true);
        SetArmed(false);
    }

    public void SetArmed(bool armed)
    {
        foreach (var rb in chunkBodies)
        {
            if (rb == null) continue;

            rb.isKinematic = !armed;      
            rb.useGravity = armed;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
