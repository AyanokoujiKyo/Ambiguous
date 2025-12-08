using UnityEngine;

public class FusePanel : MonoBehaviour
{
    [Header("Switches")]
    public ToggleSwitch s1;
    public ToggleSwitch s2;
    public ToggleSwitch s3;
    public ToggleSwitch s4;
    public ToggleSwitch s5;

    [Header("Result")]
    public GameObject whenSolvedEnable;
    public AudioSource solvedSfx;

    [Header("Lights")]
    public Light fuseLight1;
    public Light fuseLight2;
    public Light fuseLight3;

    public float light1Target = 3000f;
    public float light2Target = 3000f;
    public float light3Target = 3000f;

    public float fadeTime = 1.2f;

    bool done;

    void Start()
    {
        if (whenSolvedEnable)
            whenSolvedEnable.SetActive(false);

        InitLight(fuseLight1);
        InitLight(fuseLight2);
        InitLight(fuseLight3);
    }

    void InitLight(Light l)
    {
        if (!l) return;

        if (!l.gameObject.activeSelf)
            l.gameObject.SetActive(true);

        l.enabled = false;
        l.intensity = 0f;

        if (l.type != LightType.Directional && l.range < 8f)
            l.range = 10f;
    }

    void Update()
    {
        if (done || !s1 || !s2 || !s3 || !s4 || !s5)
            return;

        // Y, N, Y, N, Y
        if (s1.isOn && !s2.isOn && s3.isOn && !s4.isOn && s5.isOn)
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


        if (fuseLight1)
        {
            fuseLight1.enabled = true;
            StartCoroutine(Fade(fuseLight1, fuseLight1.intensity, light1Target, fadeTime));
        }

        if (fuseLight2)
        {
            fuseLight2.enabled = true;
            StartCoroutine(Fade(fuseLight2, fuseLight2.intensity, light2Target, fadeTime));
        }

        if (fuseLight3)
        {
            fuseLight3.enabled = true;
            StartCoroutine(Fade(fuseLight3, fuseLight3.intensity, light3Target, fadeTime));
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
