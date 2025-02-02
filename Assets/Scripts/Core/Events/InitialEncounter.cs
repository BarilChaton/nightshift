using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitialEncounter : MonoBehaviour
{
    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float freezeDuration = 7f;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private AudioClip stingSound;

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private string encounterDialogueText = "What the!? HEY WE'RE CLOSED!";

    private PlayerController playerController;
    private Transform playerCameraTransform;
    private AudioSource playerAudioSource;
    private bool hasTriggered = false;

    private void Awake() {
        playerController = player.gameObject.GetComponent<PlayerController>();
        playerCameraTransform = playerController.GetComponentInChildren<Camera>().transform;
        playerAudioSource = player.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !hasTriggered) 
        {
            hasTriggered = true;
            playerController.FreezeMovement();
            playerAudioSource.PlayOneShot(stingSound);

            TextMeshProUGUI textComponent = dialogueUI.GetComponent<TextMeshProUGUI>();
            textComponent.text = encounterDialogueText;
            gameManager.DisableDialogue(5f);

            StartCoroutine(ForceLookAtNPC());
        }
    }

    IEnumerator ForceLookAtNPC() {
        Vector3 directionToNpc = (NPC.transform.position - player.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNpc);

        float targetYRotation = targetRotation.eulerAngles.y;
        Quaternion newYRotation = Quaternion.Euler(0, targetYRotation, 0);

        float elapsedTime = 0f;

        while (elapsedTime < freezeDuration) {
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, newYRotation, lookSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCameraTransform.rotation = targetRotation;
        playerController.UnfreezeMovement();
        gameObject.SetActive(false);
    }
}
