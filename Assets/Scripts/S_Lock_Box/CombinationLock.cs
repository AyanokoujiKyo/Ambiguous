using UnityEngine;
using System.Collections;

public class CombinationLock : MonoBehaviour
{
    [Header("References")]
    public Dial[] dials;     
    public Transform lid;     

    [Header("Open Settings")]
    public float openAngle = -90f;    
    public float openDuration = 1f;  

    [Header("Correct Code")]
    public int[] correctCode = { 3, 7, 1 };  

    private bool unlocked = false;
    public void OnDialChanged()
    {
        if (unlocked) return;

        if (IsCodeCorrect())
        {
            unlocked = true;
            StartCoroutine(OpenLid());
        }
    }

    private bool IsCodeCorrect()
    {
        if (dials == null || dials.Length == 0) return false;
        if (correctCode == null || correctCode.Length != dials.Length) return false;

        for (int i = 0; i < dials.Length; i++)
        {
            if (dials[i].currentValue != correctCode[i])
                return false;
        }
        return true;
    }

    private IEnumerator OpenLid()
    {
        Quaternion startRot = lid.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(openAngle, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;
            lid.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }
}
