using System.Collections;
using UnityEngine;
using EasyDestuctibleWall;

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

    [Header("Breakable wall (hammer-only)")]
    [Tooltip("Trage aici WallFreezeController de pe peretele părinte (ChunkWall_NoFracture).")]
    public WallFreezeController wallFreezeController;

    [Tooltip("Dacă ai și DestructionManager pe perete, pune-l aici (opțional).")]
    public DestructionManager wallDestructionManager;

    [Tooltip("Delay mic după deschidere ca să nu atingă imediat ușa/peretele.")]
    public float armDelayAfterOpen = 0.05f;

    private bool isOpen;
    private bool animating;

    private void Awake()
    {
        if (wallDestructionManager != null)
            wallDestructionManager.SetArmed(false);
    }

    public void OpenDoor()
    {
        if (animating) return;
        if (openOnce && isOpen) return;

        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        animating = true;

        Quaternion startRot = door.localRotation;
        Vector3 axis = AxisVector();
        Quaternion endRot = startRot * Quaternion.AngleAxis(openAngle, axis);

        float t = 0f;
        float dur = Mathf.Max(0.01f, openDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            door.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        door.localRotation = endRot;
        isOpen = true;

        if (armDelayAfterOpen > 0f)
            yield return new WaitForSeconds(armDelayAfterOpen);
        if (wallFreezeController != null)
            wallFreezeController.UnlockForHammerOnlyMode();
        if (wallDestructionManager != null)
            wallDestructionManager.SetArmed(true);

        animating = false;
    }

    private Vector3 AxisVector()
    {
        switch (rotateAxis)
        {
            case Axis.X: return Vector3.right;
            case Axis.Y: return Vector3.up;
            default: return Vector3.forward;
        }
    }
}
