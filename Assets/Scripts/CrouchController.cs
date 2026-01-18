using UnityEngine;

public class CrouchController : MonoBehaviour
{
    [Header("Setari")]
    public Transform cameraOffset; // Obiectul "Camera Offset" din XR Origin
    public float distantaGhemuit = 0.6f; // Cat de mult coboram (60 cm)
    
    private bool esteGhemuit = false;
    private bool esteActivat = false; // Devine true cand deschizi usa
    private float yInitial;

    void Start()
    {
        // Tinem minte inaltimea normala de start
        if (cameraOffset != null)
        {
            yInitial = cameraOffset.localPosition.y;
        }
        else
        {
            Debug.LogError("Nu ai pus Camera Offset in script!");
        }
    }

    void Update()
    {
        // 1. Daca nu s-a deschis usa inca, nu facem nimic
        if (!esteActivat) return;

        // 2. Verificam daca apasa tasta C (pentru testare) sau Ctrl
        // (In VR e mai complicat cu butoanele, dar C merge perfect daca testezi la PC)
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }
    }

    // Aceasta functie e apelata din DoorTracker
    public void ActiveazaAbilitatea()
    {
        esteActivat = true;
        Debug.Log("Abilitatea de a te ghemui (Tasta C) a fost activata!");
    }

    // Aceasta functie face miscarea efectiva
    public void ToggleCrouch()
    {
        esteGhemuit = !esteGhemuit;

        if (cameraOffset != null)
        {
            Vector3 pozitieNoua = cameraOffset.localPosition;
            
            if (esteGhemuit)
            {
                // Coboram
                pozitieNoua.y = yInitial - distantaGhemuit;
            }
            else
            {
                // Revenim la normal
                pozitieNoua.y = yInitial;
            }

            cameraOffset.localPosition = pozitieNoua;
        }
    }
}