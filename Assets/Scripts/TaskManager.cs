using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI textAfisaj;

    [Header("Setari Timp")]
    public float timpPanaLaHint = 180f; // Timp standard (3 min)
    
    private float timer = 0f;
    private int stadiuJoc = 1; 
    // 1=Lumina, 2=Seif, 3=Cheie, 4=Usa, 5=Evadare, 6=Final

    // Aici stocam textul cu task-urile vechi terminate
    private string textIstoric = ""; 

    void Start()
    {
        ActualizeazaText("Task 1: Aprinde lumina", "");
    }

    void Update()
    {
        // Daca jocul e gata (stadiu 6), nu mai numaram timpul si nu mai dam hint-uri
        if (stadiuJoc >= 6) return;

        timer += Time.deltaTime;

        // --- LOGICA HINT-URI ---
        
        // Pentru Task 5 (Ciocan) timpul e mai scurt: 2 minute (120 secunde)
        float limitaTimp = (stadiuJoc == 5) ? 120f : timpPanaLaHint;

        if (timer >= limitaTimp)
        {
            if (stadiuJoc == 1)
                ActualizeazaText("Task 1: Aprinde lumina", "Hint: Activeaza panoul de lumini");
            else if (stadiuJoc == 2)
                ActualizeazaText("Task 2: Deschide Seiful", "Hint: Cauta numerele ascunse in camera");
            else if (stadiuJoc == 3)
                ActualizeazaText("Task 3: Cauta Cheia", "Hint: Foloseste magnetul sub calorifer");
            else if (stadiuJoc == 4)
                ActualizeazaText("Task 4: Deschide Usa", "Hint: Foloseste cheia pe clanta");
            else if (stadiuJoc == 5)
            {
                ActualizeazaText("Task 5: Evadeaza!", "Hint: Cauta un ciocan in camera");
            }
        }
    }

    // --- FUNCTII DE TRANZITIE ---

    public void FinalizeazaTask1() // Lumina -> Seif
    {
        if (stadiuJoc > 1) return;
        
        // Adaugam Task 1 la istoric ca fiind complet
        AdaugaLaIstoric("Task 1: Aprinde lumina");
        
        TreciLaStadiul(2, "Task 2: Deschide Seiful", ""); 
    }

    public void FinalizeazaTask2() // Seif -> Cheie
    {
        if (stadiuJoc > 2) return;

        AdaugaLaIstoric("Task 2: Deschide Seiful");

        TreciLaStadiul(3, "Task 3: Gaseste Cheia", "Foloseste magnetul pentru a o atrage");
    }

    public void FinalizeazaTask3() // Cheie -> Usa
    {
        if (stadiuJoc > 3) return;

        AdaugaLaIstoric("Task 3: Gaseste Cheia");

        TreciLaStadiul(4, "Task 4: Deschide Usa", "");
    }

    public void FinalizeazaTask4() // Usa Deschisa -> Sparge Peretele
    {
        if (stadiuJoc > 4) return;

        AdaugaLaIstoric("Task 4: Deschide Usa");

        TreciLaStadiul(5, "Task 5: Evadeaza!", "Cauta o metoda de a sparge peretele pentru a iesi");
    }

    // --- FUNCTIA NOUA PENTRU FINAL ---
    // Aceasta va fi apelata de scriptul EndGameTrigger de pe cubul invizibil
    public void FinalizeazaTask5()
    {
        if (stadiuJoc > 5) return;

        // Marcam ultimul task ca facut
        AdaugaLaIstoric("Task 5: Evadeaza!");
        
        stadiuJoc = 6; // Setam stadiul 6 (Game Over / Castigat)
        
        // Afisam mesajul final mare si verde
        if (textAfisaj != null)
        {
            textAfisaj.text = textIstoric + "\n<size=120%><color=green>FELICITARI! AI EVADAT!</color></size>";
        }
    }

    // Functie noua pentru a salva progresul
    void AdaugaLaIstoric(string taskVechi)
    {
        // Folosim textul "[OK]" in loc de emoji
        textIstoric += $"<s>{taskVechi}</s> <color=green> [OK]</color>\n";
    }

    // Functie ajutatoare
    void TreciLaStadiul(int stadiuNou, string textNou, string hintNou)
    {
        stadiuJoc = stadiuNou;
        timer = 0f; // Resetam ceasul
        ActualizeazaText(textNou, hintNou);
    }

    void ActualizeazaText(string taskCurent, string hint)
    {
        if (textAfisaj != null)
        {
            // Combinam istoricul cu task-ul curent
            string mesajFinal = textIstoric + taskCurent;

            if (hint != "")
            {
                mesajFinal += "\n<color=yellow>" + hint + "</color>";
            }

            textAfisaj.text = mesajFinal;
        }
    }
}