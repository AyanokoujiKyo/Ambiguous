using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CombinationLock : MonoBehaviour
{
    [Header("References")]
    public Dial[] dials;
    public Transform lid;

    [Header("Open Settings")]
    public float openAngle = -90f;
    public float openDuration = 1f;

    [Header("Correct Code (fallback)")]
    public int[] correctCode = { 3, 7, 1 };

    [Header("Code source (optional)")]
    public RandomizeDigits[] codeDisplays;  

    [Header("DEBUG")]
    [SerializeField] private string debugCode;   

    private bool unlocked = false;

    void Start()
    {
        if (codeDisplays != null &&
            dials != null &&
            codeDisplays.Length == dials.Length)
        {
            int len = dials.Length;
            correctCode = new int[len];

            for (int i = 0; i < len; i++)
            {
                var src = codeDisplays[i];
                if (src != null && src.digits != null && src.digits.Length > 0)
                {
                    correctCode[i] = src.digits[0];
                }
                else
                {
                    correctCode[i] = 0;
                }
            }
        }
        debugCode = string.Join("-", correctCode);
        UnityEngine.Debug.Log($"[CombinationLock] Cod corect: {debugCode}");
    }

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

        lid.localRotation = endRot;
    }
}
