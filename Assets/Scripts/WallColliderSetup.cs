using UnityEngine;

public class WallColliderSetup : MonoBehaviour
{
    [Header("Collider Configuration")]
    [Tooltip("Width of each wall section (adjust based on door size)")]
    public float wallSectionWidth = 1f;
    
    [Tooltip("Offset from center for left/right wall sections")]
    public float sectionOffset = 2f;
    
    [Tooltip("Height of the colliders")]
    public float colliderHeight = 2.5f;
    
    [Tooltip("Depth of the colliders")]
    public float colliderDepth = 0.1f;

    [ContextMenu("Setup Wall Colliders")]
    public void SetupColliders()
    {
        BoxCollider existingCollider = GetComponent<BoxCollider>();
        if (existingCollider != null)
        {
            DestroyImmediate(existingCollider);
            Debug.Log("Removed BoxCollider from " + gameObject.name);
        }

        Transform leftWall = transform.Find("LeftWallCollider");
        if (leftWall == null)
        {
            GameObject leftWallObj = new GameObject("LeftWallCollider");
            leftWallObj.transform.SetParent(transform);
            leftWallObj.transform.localPosition = new Vector3(-sectionOffset, 0, 0);
            leftWallObj.transform.localRotation = Quaternion.identity;
            leftWallObj.transform.localScale = Vector3.one;
            
            BoxCollider leftCollider = leftWallObj.AddComponent<BoxCollider>();
            leftCollider.size = new Vector3(wallSectionWidth, colliderHeight, colliderDepth);
            
            Debug.Log("Created LeftWallCollider");
        }

        Transform rightWall = transform.Find("RightWallCollider");
        if (rightWall == null)
        {
            GameObject rightWallObj = new GameObject("RightWallCollider");
            rightWallObj.transform.SetParent(transform);
            rightWallObj.transform.localPosition = new Vector3(sectionOffset, 0, 0);
            rightWallObj.transform.localRotation = Quaternion.identity;
            rightWallObj.transform.localScale = Vector3.one;
            
            BoxCollider rightCollider = rightWallObj.AddComponent<BoxCollider>();
            rightCollider.size = new Vector3(wallSectionWidth, colliderHeight, colliderDepth);
            
            Debug.Log("Created RightWallCollider");
        }

        Debug.Log("Wall collider setup complete!");
    }
}
