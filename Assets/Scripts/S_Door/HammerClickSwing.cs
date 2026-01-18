using System.Collections;
using UnityEngine;

public class HammerClickSwing : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Camera jucătorului (sau XR camera). Dacă e gol, folosește Camera.main.")]
    public Camera cam;

    [Tooltip("Pivotul care se rotește pentru swing (de ex: mânerul/ciocanul). Dacă e gol, folosește transformul curent.")]
    public Transform swingPivot;

    [Header("Swing animation")]
    public float swingDuration = 0.12f;
    public float swingAngle = 55f;

    [Header("Hit detection")]
    public float hitRange = 2.5f;
    public LayerMask hitMask = ~0; 

    [Header("Wall release")]
    public float breakRadius = 0.25f;
    public float impulse = 4.0f;

    private bool swinging;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        if (swingPivot == null) swingPivot = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !swinging)
        {
            StartCoroutine(SwingAndHit());
        }
    }

    private IEnumerator SwingAndHit()
    {
        swinging = true;
        DoHit();
        Quaternion start = swingPivot.localRotation;
        Quaternion down = start * Quaternion.Euler(-swingAngle, 0f, 0f);

        float half = Mathf.Max(0.01f, swingDuration * 0.5f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / half;
            swingPivot.localRotation = Quaternion.Slerp(start, down, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / half;
            swingPivot.localRotation = Quaternion.Slerp(down, start, t);
            yield return null;
        }

        swingPivot.localRotation = start;
        swinging = false;
    }

    private void DoHit()
    {
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, hitRange, hitMask, QueryTriggerInteraction.Ignore))
            return;
        var freeze = hit.collider.GetComponentInParent<WallFreezeController>();
        if (freeze != null && freeze.IsUnlocked())
        {
            Vector3 dir = ray.direction.normalized;
            freeze.ReleaseAtPoint(hit.point, breakRadius, dir * impulse);
        }
    }
}
