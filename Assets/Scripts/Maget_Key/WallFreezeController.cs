using UnityEngine;

public class WallFreezeController : MonoBehaviour
{
    [Header("Wall root (optional)")]
    [Tooltip("Dacă e gol, folosește obiectul curent ca root.")]
    public Transform wallRoot;

    [Header("Mode")]
    [Tooltip("Devine true după ce ușa s-a deschis. Până atunci, ciocanul NU poate elibera bucăți.")]
    [SerializeField] private bool unlocked = false;

    [Header("Hammer-only behavior")]
    [Tooltip("După ce ușa se deschide, peretele rămâne complet înghețat; doar ciocanul eliberează bucăți.")]
    public bool keepFrozenUntilHit = true;

    private Rigidbody[] bodies;

    private void Awake()
    {
        if (wallRoot == null) wallRoot = transform;
        bodies = wallRoot.GetComponentsInChildren<Rigidbody>(true);
        FreezeAll();
        unlocked = false;
    }

    public void UnlockForHammerOnlyMode()
    {
        unlocked = true;
        if (keepFrozenUntilHit) FreezeAll();
    }

    public bool IsUnlocked() => unlocked;

    public void FreezeAll()
    {
        if (bodies == null) return;

        foreach (var rb in bodies)
        {
            if (!rb) continue;
            rb.isKinematic = false;                
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void ReleaseChunk(Rigidbody rb)
    {
        if (!unlocked) return;
        if (!rb) return;

        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        rb.WakeUp();
    }

    public void ReleaseChunkAndPush(Rigidbody rb, Vector3 impulse)
    {
        ReleaseChunk(rb);
        if (!rb) return;
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    public void ReleaseAround(Vector3 center, float radius, Vector3 impulse)
    {
        if (!unlocked) return;
        if (bodies == null) return;

        float r2 = radius * radius;

        foreach (var rb in bodies)
        {
            if (!rb) continue;

            Vector3 p = rb.worldCenterOfMass;
            if ((p - center).sqrMagnitude > r2) continue;
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.WakeUp();
            rb.AddForce(impulse, ForceMode.Impulse);
        }
    }

    public void ReleaseAtPoint(Vector3 point, float radius, Vector3 impulse)
    {
        if (!unlocked) return;
        if (bodies == null) return;

        float r2 = radius * radius;

        foreach (var rb in bodies)
        {
            if (!rb) continue;

            Vector3 p = rb.worldCenterOfMass;
            if ((p - point).sqrMagnitude > r2) continue;

            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.WakeUp();

            if (impulse.sqrMagnitude > 0.0001f)
                rb.AddForce(impulse, ForceMode.Impulse);
        }
    }


}
