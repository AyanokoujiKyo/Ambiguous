using UnityEngine;

public class DoorTracker : MonoBehaviour
{
    public TaskManager taskManager;
    public CrouchController crouchController; // <--- LEGATURA NOUA CU SCRIPTUL DE GHEMUIT
    
    private Quaternion rotatieInitiala;
    private bool usaSDeschis = false;

    void Start()
    {
        rotatieInitiala = transform.localRotation;
    }

    void Update()
    {
        if (usaSDeschis) return; // Daca am detectat deja, nu mai verificam

        // Calculam cat de mult s-a rotit usa fata de pozitia inchisa
        float unghi = Quaternion.Angle(transform.localRotation, rotatieInitiala);

        // Daca usa s-a deschis mai mult de 10 grade
        if (unghi > 10f)
        {
            usaSDeschis = true;
            Debug.Log("Usa s-a deschis! Trecem la Task 5 si activam ghemuitul.");

            // 1. Anuntam TaskManager-ul (Schimba textul in "Sparge peretele")
            if (taskManager != null)
            {
                taskManager.FinalizeazaTask4(); 
            }

            // 2. Activam abilitatea de a te ghemui (ca sa poti lua ciocanul)
            if (crouchController != null)
            {
                crouchController.ActiveazaAbilitatea(); // <--- AICI SE ACTIVEAZA TASTA C
            }
        }
    }
}