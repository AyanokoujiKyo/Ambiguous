using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public float openSpeed = 1.5f;
    bool opened;
    Quaternion closedRot, targetRot;

    void Start()
    {
        closedRot = transform.localRotation;
        targetRot = closedRot;
    }

    public void Open()
    {
        if (opened) return;
        opened = true;
        targetRot = closedRot * Quaternion.Euler(openRotation);
        StopAllCoroutines();
        StartCoroutine(OpenCo());
    }

    System.Collections.IEnumerator OpenCo()
    {
        Quaternion start = transform.localRotation;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            transform.localRotation = Quaternion.Slerp(start, targetRot, t);
            yield return null;
        }
        transform.localRotation = targetRot;
    }
}
