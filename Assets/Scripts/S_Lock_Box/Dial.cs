using UnityEngine;
using TMPro;

public class Dial : MonoBehaviour
{
    [Header("Config")]
    public int maxValue = 10;
    public float stepAngle = 36f;
    public int currentValue = 0;

    [Header("References")]
    public CombinationLock lockManager;
    public TextMeshPro label;

    private void Start()
    {
        if (label != null)
            label.text = currentValue.ToString();
    }

    public void RotateStep()
    {
        currentValue = (currentValue + 1) % maxValue;
        transform.Rotate(0f, stepAngle, 0f, Space.Self);

        if (label != null)
            label.text = currentValue.ToString();

        if (lockManager != null)
            lockManager.OnDialChanged();
    }
}
