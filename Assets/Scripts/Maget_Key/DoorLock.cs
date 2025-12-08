using System.Collections;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [Header("Door")]
    public Transform door;
    public Axis rotateAxis = Axis.Y;  
    public float openAngle = 90f;
    public float openDuration = 1f;

    [Header("Options")]
    public bool openOnce = true;

    bool isOpen;
    bool animating;

    public void OpenDoor()
    {
        if (animating) return;
        if (openOnce && isOpen) return;

        StartCoroutine(OpenRoutine());
    }

    [ContextMenu("Test Open")]
    void TestOpen() => OpenDoor();

    IEnumerator OpenRoutine()
    {
        animating = true;

        Quaternion startRot = door.localRotation;
        Vector3 axis = AxisVector();
        Quaternion endRot = startRot * Quaternion.AngleAxis(openAngle, axis);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;
            door.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        door.localRotation = endRot;
        isOpen = true;
        animating = false;
    }

    Vector3 AxisVector()
    {
        switch (rotateAxis)
        {
            case Axis.X: return Vector3.right;
            case Axis.Y: return Vector3.up;
            default: return Vector3.forward; // Z
        }
    }
}
