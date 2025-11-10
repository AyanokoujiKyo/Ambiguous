using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[DisallowMultipleComponent]
public class ToggleSwitch : MonoBehaviour
{
    public enum Axis { X, Y, Z }
    public bool isOn;
    public Axis rotateAxis = Axis.X;
    public float onAngle = 90f;
    public float offAngle = 0f;
    public float snapTime = 0.08f;
    public bool toggleOnSelectExit = true;

    XRGrabInteractable grab;
    Rigidbody rb;
    Quaternion baseRot;
    Coroutine snapCo;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        if (rb) { rb.useGravity = false; rb.isKinematic = true; }
        baseRot = transform.localRotation;
        ApplyInstant();
    }

    void OnEnable()
    {
        if (!grab) return;
        grab.trackPosition = false;
        grab.trackRotation = false;
        grab.selectEntered.RemoveAllListeners();
        grab.selectExited.RemoveAllListeners();
        if (toggleOnSelectExit) grab.selectExited.AddListener(_ => Toggle());
        else grab.selectEntered.AddListener(_ => Toggle());
    }

    void OnDisable()
    {
        if (!grab) return;
        grab.selectEntered.RemoveAllListeners();
        grab.selectExited.RemoveAllListeners();
    }

    public void Toggle()
    {
        isOn = !isOn;
        ApplySmooth();
    }

    void ApplyInstant()
    {
        transform.localRotation = TargetRotation();
    }

    void ApplySmooth()
    {
        if (snapTime <= 0f) { ApplyInstant(); return; }
        if (snapCo != null) StopCoroutine(snapCo);
        snapCo = StartCoroutine(SnapTo(TargetRotation(), snapTime));
    }

    Quaternion TargetRotation()
    {
        Vector3 axis = rotateAxis == Axis.X ? Vector3.right : rotateAxis == Axis.Y ? Vector3.up : Vector3.forward;
        float a = isOn ? onAngle : offAngle;
        return baseRot * Quaternion.AngleAxis(a, axis);
    }

    System.Collections.IEnumerator SnapTo(Quaternion target, float time)
    {
        Quaternion start = transform.localRotation;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / time);
            transform.localRotation = Quaternion.Slerp(start, target, k);
            yield return null;
        }
        transform.localRotation = target;
        snapCo = null;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying) return;
        baseRot = transform.localRotation;
        transform.localRotation = TargetRotation();
    }
#endif
}
