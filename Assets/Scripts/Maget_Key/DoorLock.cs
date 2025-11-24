using System.Collections;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Header("Door")]
    public Transform door;          
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
    void TestOpen()
    {
        OpenDoor();
    }


    IEnumerator OpenRoutine()
    {
        animating = true;

        Quaternion startRot = door.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, openAngle, 0f);

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
}
