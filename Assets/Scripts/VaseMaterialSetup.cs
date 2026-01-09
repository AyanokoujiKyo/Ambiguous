using UnityEngine;

public class VaseMaterialSetup : MonoBehaviour
{
    [Header("Vase Colors")]
    [ColorUsage(false)]
    public Color vaseColor = new Color(0.2f, 0.6f, 0.8f, 1f);
    public float vaseSmoothness = 0.85f;
    public float vaseMetallic = 0.1f;

    [Header("Plant Colors")]
    [ColorUsage(false)]
    public Color plantColor = new Color(0.15f, 0.65f, 0.2f, 1f);
    public float plantSmoothness = 0.3f;

    [Header("Soil Colors")]
    [ColorUsage(false)]
    public Color soilColor = new Color(0.3f, 0.2f, 0.15f, 1f);
    public float soilSmoothness = 0.1f;

    [ContextMenu("Apply Beautiful Materials")]
    public void ApplyMaterials()
    {
        Transform vaseTransform = transform.Find("Vase");
        if (vaseTransform != null)
        {
            Material vaseMaterial = CreateMaterial("VaseCeramic", vaseColor, vaseSmoothness, vaseMetallic);
            ApplyMaterialToObject(vaseTransform.gameObject, vaseMaterial);
            Debug.Log("Applied ceramic material to Vase");
        }

        Transform plantTransform = transform.Find("Decorative Plant");
        if (plantTransform != null)
        {
            Material plantMaterial = CreateMaterial("PlantGreen", plantColor, plantSmoothness, 0f);
            ApplyMaterialToObject(plantTransform.gameObject, plantMaterial);
            Debug.Log("Applied green material to Plant");
        }

        Transform soilTransform = transform.Find("Soil");
        if (soilTransform != null)
        {
            Material soilMaterial = CreateMaterial("Soil", soilColor, soilSmoothness, 0f);
            ApplyMaterialToObject(soilTransform.gameObject, soilMaterial);
            Debug.Log("Applied soil material to Soil");
        }

        Debug.Log("Decorative Vase materials applied successfully!");
    }

    private Material CreateMaterial(string materialName, Color color, float smoothness, float metallic)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.name = materialName;
        mat.SetColor("_BaseColor", color);
        mat.SetFloat("_Smoothness", smoothness);
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_SpecularHighlights", 1f);
        mat.SetFloat("_EnvironmentReflections", 1f);
        
        return mat;
    }

    private void ApplyMaterialToObject(GameObject obj, Material material)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Material[] materials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = material;
            }
            renderer.sharedMaterials = materials;
        }
    }
}
