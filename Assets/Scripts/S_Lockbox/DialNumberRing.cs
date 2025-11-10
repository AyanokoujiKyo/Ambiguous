using UnityEngine;
using TMPro;

public class DialNumberRing : MonoBehaviour
{
    public float radius = 0.12f;
    public float height = 0.02f;
    public TMP_FontAsset font;
    public float fontSize = 0.08f;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            float ang = Mathf.Deg2Rad * (i * 36f);
            Vector3 pos = new Vector3(Mathf.Cos(ang) * radius, height, Mathf.Sin(ang) * radius);
            var go = new GameObject("N" + i, typeof(TextMeshPro));
            go.transform.SetParent(transform, false);
            go.transform.localPosition = pos;
            go.transform.LookAt(transform.position + Vector3.up * height); 
            go.transform.Rotate(0, 180, 0); 

            var tmp = go.GetComponent<TextMeshPro>();
            if (font) tmp.font = font;
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.text = i.ToString();
        }
    }
}
