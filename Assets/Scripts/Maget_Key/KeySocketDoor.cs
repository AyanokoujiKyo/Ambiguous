using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KeySocketDoor : MonoBehaviour
{
    public DoorLock doorLock;                
    public XRSocketInteractor socket;       
    public string requiredKeyTag = "Key";    

    void Awake()
    {
        if (!socket)
            socket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        if (!socket) return;
        socket.selectEntered.AddListener(OnSelectEntered);
    }

    void OnDisable()
    {
        if (!socket) return;
        socket.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!doorLock) return;

        GameObject keyGO = args.interactableObject.transform.gameObject;


        if (!string.IsNullOrEmpty(requiredKeyTag) &&
            !keyGO.CompareTag(requiredKeyTag))
            return;

        doorLock.OpenDoor();
    }
}
