using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public bool CanInteract = true;
    public Transform interactorSource;
    public float interactRange;
    public bool isHoldingInventory = false;

    [SerializeField] private InputActionAsset PlayerActions;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private string dialogueBlockToDisplay = "I should get the task list first.";

    private InputAction interactAction;
    private InteractableObject lastInteractObject = null;

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
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange)) {
            if (hitInfo.collider.gameObject.TryGetComponent(out InteractableObject interactObject)) {

                if (lastInteractObject != interactObject) {
                    lastInteractObject?.OnLoseFocus();
                    lastInteractObject = interactObject;
                }

                interactObject.OnFocus();

                if (interactionPressed) {
                    // If player is trying to open a door or interact with non inventory
                    if (!interactObject.CompareTag("InventoryItem") && !interactObject.CompareTag("Container")) {
                        interactObject.OnInteract();
                    } 

                    // If player is trying to pick up inventory item
                    if (interactObject.CompareTag("InventoryItem") && !isHoldingInventory && gameManager.stickyNotesPicked) {
                        isHoldingInventory = true;
                        interactObject.OnInteract();
                    }

                    if (interactObject.CompareTag("InventoryItem") && !gameManager.stickyNotesPicked) {
                        TextMeshProUGUI textComponent = dialogueUI.GetComponent<TextMeshProUGUI>();
                        textComponent.text = dialogueBlockToDisplay;
                        gameManager.DisableDialogue(5f);
                    }

                    if (interactObject.CompareTag("StickyNote") && !gameManager.stickyNotesPicked) {
                        interactObject.OnInteract();
                    }
                    
                    // If player wants to throw inventory item to the dump container
                    if ((interactObject.CompareTag("Container") || interactObject.CompareTag("ToiletContainer")) && isHoldingInventory) {
                        isHoldingInventory = false;
                        interactObject.OnInteract();
                    }
                }

            } else {
                if (lastInteractObject != null) {
                    lastInteractObject.OnLoseFocus();
                    lastInteractObject = null;
                }
            }
        } else {
            if (lastInteractObject != null) {
                lastInteractObject.OnLoseFocus();
                lastInteractObject = null;
            }
        }
    }
}
