using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCCMove : MonoBehaviour
{
    public float moveSpeed = 2f;

    CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // input clasic: WASD / săgeți
        float h = Input.GetAxisRaw("Horizontal");  // A / D sau stânga / dreapta
        float v = Input.GetAxisRaw("Vertical");    // W / S sau sus / jos

        Vector3 input = new Vector3(h, 0f, v);
        if (input.sqrMagnitude < 0.0001f) return;

        // direcție relativă la orientarea camerei
        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * v + right * h).normalized;

        cc.Move(moveDir * moveSpeed * Time.deltaTime);
    }
}
