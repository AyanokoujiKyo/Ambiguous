using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(XRGrabInteractable))]
public class DialDigitNumbered : MonoBehaviour
{
    public enum Axis { X, Y, Z }
    public Axis rotateAxis = Axis.Y;
    public int value;

    [Range(0.01f, 1f)] public float radius = 0.12f;
    [Range(-0.5f, 0.5f)] public float height = 0.05f;
    [Range(0.01f, 1f)] public float labelScale = 0.15f;
    [Range(0.04f, 1f)] public float fontSize = 0.18f;
    public Color fontColor = Color.white;
    public Color outlineColor = Color.black;
    [Range(0f, 1f)] public float outlineWidth = 0.3f;
    public bool billboard = true;

    XRGrabInteractable grab;
    Rigidbody rb;
    Quaternion baseRot;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        grab = GetComponent<XRGrabInteractable>();
        grab.movementType = XRBaseInteractable.MovementType.Kinematic;
        grab.trackPosition = false;
        grab.trackRotation = true;
        grab.throwOnDetach = false;
    }

    void Start()
    {
        baseRot = transform.localRotation;
        Regenerate();
        SnapFromRotation();
        grab.selectExited.AddListener(_ => SnapFromRotation());
    }

    void LateUpdate()
    {
        if (!billboard) return;
        if (!cam) cam = Camera.main;
        if (!cam) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if (!t.name.StartsWith("N")) continue;
            Vector3 dir = t.position - cam.transform.position;
            t.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            if (transform.GetChild(i).name.StartsWith("N"))
                DestroyImmediate(transform.GetChild(i).gameObject);

        for (int i = 0; i < 10; i++)
        {
            float ang = Mathf.Deg2Rad * (i * 36f);
            Vector3 pos = new Vector3(Mathf.Cos(ang) * radius, height, Mathf.Sin(ang) * radius);

            var go = new GameObject("N" + i, typeof(TextMeshPro));
            go.transform.SetParent(transform, false);
            go.transform.localPosition = pos;
            go.transform.localScale = Vector3.one * labelScale;

            var tmp = go.GetComponent<TextMeshPro>();
            tmp.text = i.ToString();
            tmp.fontSize = fontSize;
            tmp.color = fontColor;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;
            tmp.outlineColor = outlineColor;
            tmp.outlineWidth = outlineWidth;

            var r = tmp.GetComponent<Renderer>();
            if (r) r.sortingOrder = 500;
        }
    }

    public void SnapFromRotation()
    {
        Vector3 axis = rotateAxis == Axis.X ? Vector3.right :
                       rotateAxis == Axis.Y ? Vector3.up : Vector3.forward;

        Quaternion rel = Quaternion.Inverse(baseRot) * transform.localRotation;
        rel.ToAngleAxis(out float angle, out Vector3 a);
        if (Vector3.Dot(a, axis) < 0) angle = -angle;
        angle = Mathf.DeltaAngle(0f, angle);

        const float step = 36f;
        float snapped = Mathf.Round(angle / step) * step;
        transform.localRotation = baseRot * Quaternion.AngleAxis(snapped, axis);
        value = ((int)Mathf.Round(snapped / step) % 10 + 10) % 10;
    }
}
