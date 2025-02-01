using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public bool CanInteract = true;
    public Transform interactorSource;
    public float interactRange;
    public bool isHoldingInventory = false;

    [SerializeField] private InputActionAsset PlayerActions;

    private InputAction interactAction;

    private void Awake() {
        interactAction = PlayerActions.FindActionMap("Player").FindAction("Interact");
    }

    void Update()
    {
        if (CanInteract) {
            Interact();
        }
    }

    private void OnEnable() {
        interactAction.Enable();
    }

    private void OnDisable() {
        interactAction.Disable();
    }

    private void Interact() {
        bool interactionPressed = interactAction.WasPressedThisFrame();
        if (!interactionPressed) return;

        if (interactionPressed) {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange)) {
                if (hitInfo.collider.gameObject.TryGetComponent(out InteractableObject interactObject)) {

                    // If player is trying to open a door or interact with non inventory
                    if (!interactObject.CompareTag("InventoryItem") && !interactObject.CompareTag("Container")) {
                        interactObject.OnInteract();
                    } 

                    // If player is trying to pick up inventory item
                    if (interactObject.CompareTag("InventoryItem") && !isHoldingInventory) {
                        isHoldingInventory = true;
                        interactObject.OnInteract();
                    }
                    
                    // If player wants to throw inventory item to the dump container
                    if ((interactObject.CompareTag("Container") || interactObject.CompareTag("ToiletContainer")) && isHoldingInventory) {
                        isHoldingInventory = false;
                        interactObject.OnInteract();
                    }
                }
            }
        }
    }
}
