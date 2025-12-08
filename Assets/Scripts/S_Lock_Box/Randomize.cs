using UnityEngine;
using TMPro;

public class RandomizeDigits : MonoBehaviour
{
    [Header("Text objects (0-9)")]
    public TMP_Text[] digitTexts;

    [HideInInspector]
    public int[] digits;  

    void Awake()
    {
        digits = new int[digitTexts.Length];

        for (int i = 0; i < digitTexts.Length; i++)
        {
            if (digitTexts[i] == null) continue;

            int r = UnityEngine.Random.Range(0, 10); 
            digits[i] = r;
            digitTexts[i].text = r.ToString();
        }
    }
}
