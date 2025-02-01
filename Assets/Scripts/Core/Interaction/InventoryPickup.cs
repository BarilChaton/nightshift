using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenotryPickup : InteractableObject {
    [Header("Linked player objects")]
    [SerializeField] private GameObject playerInventoryItem;

    [Header("Audio")]
    [SerializeField] private AudioClip trashPickup;
    [SerializeField] private AudioClip detergentPickup;

    [SerializeField] private GameManager gameManager;
    public bool isToiletTrash = false;

    public override void OnFocus() {

    }

    public override void OnInteract() {
        if (!gameManager.stickyNotesPicked) return;

        gameObject.SetActive(false);
        playerInventoryItem.SetActive(true);

        AudioSource audioSource = playerInventoryItem.GetComponentInParent<AudioSource>();

        if (gameObject.name == "Garbage_bag" || gameObject.name == "_toiletTrash") {
            audioSource.PlayOneShot(trashPickup);

            if (isToiletTrash) {
                gameManager.ActivateNPCOnWindow();
            }

        } else if (gameObject.name == "Chemical_01") {
            audioSource.PlayOneShot(detergentPickup);
        }

        if (gameObject.name == "_toiletTrash") {
            gameManager.toiletTrashPicked = true;
        }
    }

    public override void OnLoseFocus() {

    }
}
