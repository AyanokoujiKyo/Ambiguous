using UnityEngine;
// Acest namespace e necesar ca sa lucram cu sistemul de interactiune VR
using UnityEngine.XR.Interaction.Toolkit; 

public class KeyLogic : MonoBehaviour
{
    public TaskManager taskManager;
    private bool aFostGasita = false;

    // Aceasta functie va fi apelata automat de Unity cand prinzi cheia
    public void OnGrab()
    {
        if (aFostGasita) return; // Daca am gasit-o deja, nu mai facem nimic

        aFostGasita = true;
        
        if (taskManager != null)
        {
            taskManager.FinalizeazaTask3(); // Trecem la Task 4
        }
    }
}