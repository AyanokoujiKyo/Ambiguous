using UnityEngine;

public class ClickBreaker : MonoBehaviour
{
    public float range = 5f; // Distanta maxima

    void Update()
    {
        // Click Stanga (0)
        if (Input.GetMouseButtonDown(0))
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Am lovit: " + hit.transform.name);

            // Cautam scriptul BreakableWall pe obiectul lovit sau pe parintele lui
            BreakableWall wall = hit.transform.GetComponent<BreakableWall>();
            if (wall == null)
            {
                wall = hit.transform.GetComponentInParent<BreakableWall>();
            }

            // Daca am gasit peretele, il lovim
            if (wall != null)
            {
                wall.Hit(); // Asta apeleaza logica ta cu hits++
            }
        }
    }
}