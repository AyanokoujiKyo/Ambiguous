using UnityEngine;

public class HammerProximityHit : MonoBehaviour
{
    public float minSwingSpeed = 0.6f;  
    public float cooldown = 0.35f;       
    private float lastHitTime = -999f;

    private Vector3 lastPos;
    private float speed;

    void Update()
    {
        speed = (transform.position - lastPos).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPos = transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        if (Time.time - lastHitTime < cooldown) return;
        if (speed < minSwingSpeed) return;

        var wall = other.GetComponentInParent<BreakableWall>();
        if (wall == null) return;

        lastHitTime = Time.time;
        StartCoroutine(Kick());

        wall.Hit();
    }

    System.Collections.IEnumerator Kick()
    {
        var t = transform;
        Vector3 start = t.localPosition;
        Vector3 back = start + (-t.forward) * 0.02f;

        float dur = 0.06f;
        float x = 0f;
        while (x < dur)
        {
            x += Time.deltaTime;
            float a = x / dur;
            t.localPosition = Vector3.Lerp(start, back, a);
            yield return null;
        }
        t.localPosition = start;
    }
}
