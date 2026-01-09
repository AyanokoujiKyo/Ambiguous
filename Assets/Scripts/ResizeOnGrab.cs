using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResizeOnGrab : MonoBehaviour
{
    [Header("Ce obiect marim?")]
    public Transform modelVizual; // Asigura-te ca SM_Key e pus aici in Inspector

    [Header("Setari Marime")]
    public Vector3 marimeInMana = new Vector3(4, 4, 4);
    public Vector3 marimeJos = new Vector3(1, 1, 1);

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            // Leaga automat functiile
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnDrop);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnDrop);
        }
    }

    // Am facut functiile PUBLIC ca sa le vedem daca e nevoie
    public void OnGrab(SelectEnterEventArgs args)
    {
        if (modelVizual != null)
        {
            modelVizual.localScale = marimeInMana;
            Debug.Log(">>> GRAB: Am marit SM_Key!"); 
        }
        else
        {
            Debug.LogError("LIPSESTE MODELUL VIZUAL! Trage SM_Key in script.");
        }
    }

    public void OnDrop(SelectExitEventArgs args)
    {
        if (modelVizual != null)
        {
            modelVizual.localScale = marimeJos;
            Debug.Log("<<< DROP: Am micsorat SM_Key!");
        }
    }
}