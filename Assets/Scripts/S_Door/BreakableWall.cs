using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("Wall States")]
    public GameObject wallIntact;
    public GameObject wallCracked;
    public GameObject wallBroken;

    [Header("Optional Objects")]
    public GameObject textToHide;

    [Header("VFX (Particles)")]
    public ParticleSystem hitVfx;      
    public ParticleSystem breakVfx;    
    public Transform vfxPoint;        

    [Header("Settings")]
    public int hitsToBreak = 3;

    private int hits = 0;
    private bool isBroken = false;

    void Start()
    {
        hits = 0;
        isBroken = false;

        if (wallIntact != null) wallIntact.SetActive(true);
        if (wallCracked != null) wallCracked.SetActive(false);
        if (wallBroken != null) wallBroken.SetActive(false);

        if (textToHide != null) textToHide.SetActive(true);
    }

    public void Hit()
    {
        if (isBroken) return;

        hits++;

        PlayVfx(hitVfx);

        if (hits == 1)
        {
            if (wallIntact != null) wallIntact.SetActive(false);
            if (wallCracked != null) wallCracked.SetActive(true);
        }
        else if (hits >= hitsToBreak)
        {
            isBroken = true;

            if (wallIntact != null) wallIntact.SetActive(false);
            if (wallCracked != null) wallCracked.SetActive(false);
            if (wallBroken != null) wallBroken.SetActive(true);

            if (textToHide != null) textToHide.SetActive(false);

            PlayVfx(breakVfx);
        }
    }

    private void PlayVfx(ParticleSystem vfx)
    {
        if (vfx == null) return;

        if (vfxPoint != null)
            vfx.transform.position = vfxPoint.position;

        vfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        vfx.Emit(25); 
    }

}
