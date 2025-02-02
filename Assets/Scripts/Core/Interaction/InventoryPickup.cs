using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvenotryPickup : InteractableObject {
    [Header("Linked player objects")]
    [SerializeField] private GameObject playerInventoryItem;

    [Header("Audio")]
    [SerializeField] private AudioClip trashPickup;
    [SerializeField] private AudioClip detergentPickup;

    [SerializeField] private GameManager gameManager;
    public bool isToiletTrash = false;

    [SerializeField] private GameObject interactorText;
    [SerializeField] private string textToDisplay = "Test";   

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private string dialogueTrashToDisplay = "Better throw this in the dump outside.";
    [SerializeField] private string dialogueDetergentToDisplay = "Better pour this in the toilet.";

    public override void OnFocus() {
        TextMeshProUGUI textComponent = interactorText.GetComponent<TextMeshProUGUI>();
        textComponent.text = textToDisplay;
    }

    public override void OnInteract() {
        gameObject.SetActive(false);
        playerInventoryItem.SetActive(true);

        AudioSource audioSource = playerInventoryItem.GetComponentInParent<AudioSource>();

        if (gameObject.name == "Garbage_bag" || gameObject.name == "_toiletTrash") {
            audioSource.PlayOneShot(trashPickup);

            if (dialogueUI != null) {
                TextMeshProUGUI textComponent = dialogueUI.GetComponent<TextMeshProUGUI>();
                textComponent.text = dialogueTrashToDisplay;
                gameManager.DisableDialogue(5f);
            }

            if (isToiletTrash) {
                gameManager.ActivateNPCOnWindow();
            }

        } else if (gameObject.name == "Chemical_01") {
            audioSource.PlayOneShot(detergentPickup);

            if (dialogueUI != null) {
                TextMeshProUGUI textComponent = dialogueUI.GetComponent<TextMeshProUGUI>();
                textComponent.text = dialogueDetergentToDisplay;
                gameManager.DisableDialogue(5f);
            }
        }

        if (gameObject.name == "_toiletTrash") {
            gameManager.toiletTrashPicked = true;
        }
    }

    public override void OnLoseFocus() {
        TextMeshProUGUI textComponent = interactorText.GetComponent<TextMeshProUGUI>();
        textComponent.text = "";
    }
}
