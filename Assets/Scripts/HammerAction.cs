using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// Daca ai eroare la linia de jos, sterge-o (Unity 6 / XR Toolkit nou o cere)
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class HammerAction : MonoBehaviour
{
    public enum Axa { X, Y, Z }

    [Header("Setari Vizuale")]
    public Transform modelVizual; // Ataseaza 'Attach' aici
    public Axa axaRotatie = Axa.X; 
    public float unghiLovire = 90f; 
    public float viteza = 2f; 

    [Header("Feel")]
    public AnimationCurve curbaLoviturii = new AnimationCurve(
        new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));

    [Header("Debug")]
    public bool isSwinging = false; 

    private XRGrabInteractable grabInteractable;
    private Quaternion rotatieInitiala;
    private bool animatieInCurs = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        if (modelVizual != null)
            rotatieInitiala = modelVizual.localRotation;
            
        // Siguranta: ciocanul nu ataca la start
        isSwinging = false;
        animatieInCurs = false;
    }

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.activated.AddListener(OnLovituraVR);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.activated.RemoveListener(OnLovituraVR);
    }

    void Update()
    {
        // Input pentru testare PC (Space sau Click Dreapta)
        if (grabInteractable.isSelected)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            {
                PornesteAnimatia();
            }
        }
    }

    private void OnLovituraVR(ActivateEventArgs args)
    {
        PornesteAnimatia();
    }

    private void PornesteAnimatia()
    {
        if (!animatieInCurs && modelVizual != null)
        {
            StartCoroutine(ExecutaAnimatieNaturala());
        }
    }

    private IEnumerator ExecutaAnimatieNaturala()
    {
        animatieInCurs = true;
        isSwinging = true; // <--- ACUM devine periculos

        float timp = 0;
        
        Vector3 axaVector = Vector3.right; // X
        if (axaRotatie == Axa.Y) axaVector = Vector3.up;
        if (axaRotatie == Axa.Z) axaVector = Vector3.forward;

        while (timp < 1f)
        {
            timp += Time.deltaTime * viteza;
            float valoareCurba = curbaLoviturii.Evaluate(timp);
            
            Quaternion rotatiePlus = Quaternion.AngleAxis(valoareCurba * unghiLovire, axaVector);
            modelVizual.localRotation = rotatieInitiala * rotatiePlus;

            yield return null;
        }

        // Resetare perfecta
        modelVizual.localRotation = rotatieInitiala;
        
        isSwinging = false; // <--- Gata pericolul
        animatieInCurs = false;
    }

    // --- PAZA LA COLIZIUNE ---
    private void OnCollisionEnter(Collision collision)
    {
        // 1. Daca NU atacam (fara Click/Space), ignoram.
        if (isSwinging == false) return;

        // 2. Cautam scriptul peretelui PE OBIECTUL LOVIT sau PE PARINTELE LUI
        // Asta rezolva problema cand lovesti o caramida mica
        BreakableWall perete = collision.gameObject.GetComponentInParent<BreakableWall>();
        
        // Daca nu l-am gasit pe parinte, incercam si direct pe obiect
        if (perete == null) 
            perete = collision.gameObject.GetComponent<BreakableWall>();
        
        if (perete != null)
        {
            perete.Hit();
            isSwinging = false; // Oprim atacul dupa primul impact
        }
    }
}