using UnityEngine;

public class FusePanel : MonoBehaviour
{
    [Header("Switches")]
    public ToggleSwitch s1;
    public ToggleSwitch s2;
    public ToggleSwitch s3;
    public ToggleSwitch s4;

    [Header("Result")]
    public GameObject whenSolvedEnable;
    public AudioSource solvedSfx;
    public Light fuseLight;
    public float lightTarget = 3000f;
    public float fadeTime = 1.2f;

    bool done;

    void Start()
    {
        if (whenSolvedEnable)
            whenSolvedEnable.SetActive(false);

        if (fuseLight)
        {
            if (!fuseLight.gameObject.activeSelf)
                fuseLight.gameObject.SetActive(true);

            fuseLight.enabled = false;
            fuseLight.intensity = 0f;

            if (fuseLight.type != LightType.Directional && fuseLight.range < 8f)
                fuseLight.range = 10f;
        }
    }

    void Update()
    {
        if (done || !s1 || !s2 || !s3 || !s4)
            return;

        if (s1.isOn && !s2.isOn && s3.isOn && !s4.isOn)
        {
            Resolve();
        }
    }

    void Resolve()
    {
        done = true;

        if (whenSolvedEnable)
            whenSolvedEnable.SetActive(true);

        if (solvedSfx)
            solvedSfx.Play();

        if (fuseLight)
        {
            fuseLight.enabled = true;
            StopAllCoroutines();
            StartCoroutine(Fade(fuseLight, fuseLight.intensity, lightTarget, fadeTime));
        }
    }

    System.Collections.IEnumerator Fade(Light l, float a, float b, float t)
    {
        float s = 0f;
        while (s < t)
        {
            s += Time.deltaTime;
            l.intensity = Mathf.Lerp(a, b, s / t);
            yield return null;
        }
        l.intensity = b;
    }

    [ContextMenu("Test Unlock")]
    void TestUnlock() => Resolve();
}
