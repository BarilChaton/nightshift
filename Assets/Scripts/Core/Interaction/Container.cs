using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Container : InteractableObject {
    [SerializeField] private GameObject inventoryItem;
    [SerializeField] private GameObject toiletTrashObjective = null;
    [SerializeField] private GameObject detergentObjective = null;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioClip dumpClose;
    [SerializeField] private AudioClip toiletCleaned;

    private AudioSource audioSource;

    public override void OnFocus() {

    }

    public override void OnInteract() {
        inventoryItem.SetActive(false);
        UpdateGameManager();
    }

    public override void OnLoseFocus() {

    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void UpdateGameManager() {
        if (gameObject.CompareTag("Container")) {
            gameManager.trashBagsDisposed++;
            audioSource.PlayOneShot(dumpClose);
        }

        if (gameObject.CompareTag("Container") && gameManager.toiletTrashPicked) {
            gameManager.toiletTrashDisposed = true;

            if (toiletTrashObjective != null) {
                toiletTrashObjective.gameObject.SetActive(true);
            }

            if (!gameManager.sequenceOneDone) {
                gameManager.RunSequenceOne();
            }

            if (gameManager.toiletDetergentAdded && gameManager.toiletTrashDisposed) {
                gameManager.RandomizeGroups();
            }

            audioSource.PlayOneShot(dumpClose);
        }

        if (gameObject.CompareTag("ToiletContainer")) {
            gameManager.toiletDetergentAdded = true;

            if (detergentObjective != null) {
                detergentObjective.gameObject.SetActive(true);
            }

            audioSource.PlayOneShot(toiletCleaned);

            if (gameManager.toiletDetergentAdded && gameManager.toiletTrashDisposed) {
                gameManager.RandomizeGroups();
            }
        }
    }
}
