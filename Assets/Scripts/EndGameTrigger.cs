using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public TaskManager taskManager;
    private bool jocTerminat = false;

    private void OnTriggerEnter(Collider other)
    {
        if (jocTerminat) return;

        string nume = other.name;
        Debug.Log("Trigger atins de: " + nume);

        // EXCLUDEM PIETRELE SI CIOCANUL
        if (nume.Contains("Chunk") || nume.Contains("Brick") || nume.Contains("Hammer"))
        {
            return; 
        }

        // Daca ajungem aici, e Jucatorul (Main Camera sau XR Origin)
        jocTerminat = true;
        Debug.Log("VICTORIE! Declansat de: " + nume);

        if (taskManager != null)
        {
            taskManager.FinalizeazaTask5();
        }
    }
}