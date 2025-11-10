using UnityEngine;
using UnityEngine.Events;

public class DialLock : MonoBehaviour
{
    public DialDigitNumbered d1, d2, d3;
    public Vector3Int correct = new Vector3Int(4, 1, 7);
    public UnityEvent onUnlocked;
    public UnityEvent onWrong;

    void Update()
    {
        if (!d1 || !d2 || !d3) return;

        if (d1.value == correct.x && d2.value == correct.y && d3.value == correct.z)
            onUnlocked?.Invoke();
        else
            onWrong?.Invoke();
    }
}
