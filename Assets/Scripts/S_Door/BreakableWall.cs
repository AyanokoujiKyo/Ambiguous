using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("Referinte Obligatorii")]
    public GameObject wallIntact; // Trage aici obiectul Wall_Intact (peretele intreg)
    public GameObject wallBroken; // Trage aici obiectul Wall_Broken (bucatile)

    [Header("Setari")]
    public int hitsToBreak = 3; 
    public float timpIntreLovituri = 0.5f;

    private int hits = 0;
    private bool isBroken = false;
    private float ultimulHitTime = 0f;

    void Start()
    {
        hits = 0;
        isBroken = false;

        // LA START: 
        // 1. Activam peretele intreg ca sa se vada frumos
        if (wallIntact != null) wallIntact.SetActive(true);
        
        // 2. Dezactivam peretele spart (sa nu se vada crapaturile inca)
        if (wallBroken != null) wallBroken.SetActive(false);
    }

    public void Hit()
    {
        if (isBroken) return;
        if (Time.time - ultimulHitTime < timpIntreLovituri) return;

        hits++;
        ultimulHitTime = Time.time;
        Debug.Log("Lovitura! " + hits + "/" + hitsToBreak);

        if (hits >= hitsToBreak)
        {
            BreakTheWall();
        }
    }

    // Functie publica pentru testare rapida (Click Dreapta pe script -> Test Break)
    [ContextMenu("Test Break")] 
    public void BreakTheWall()
    {
        isBroken = true;
        Debug.Log("PERETE PRABUSIT!");

        // PASUL CRUCIAL: Ascundem peretele intreg!
        if (wallIntact != null) wallIntact.SetActive(false);

        // Activam bucatile si le dam drumul la fizica
        if (wallBroken != null)
        {
            wallBroken.SetActive(true);
            
            Rigidbody[] rbs = wallBroken.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody rb in rbs)
            {
                rb.isKinematic = false; // Porneste fizica
                rb.useGravity = true;   // Porneste gravitatia
                // Optional: O mica forta ca sa nu cada doar in jos
                rb.AddExplosionForce(100f, transform.position, 2f); 
            }
        }
    }
}