using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialEncounter : MonoBehaviour
{
    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject player;
    [SerializeField] private float freezeDuration = 3f;
    [SerializeField] private float lookSpeed = 0.1f;
    private PlayerController playerController;
    private Transform playerCameraTransform;
    private bool hasTriggered = false;

    private void Awake() {
        playerController = player.gameObject.GetComponent<PlayerController>();
        playerCameraTransform = playerController.GetComponentInChildren<Camera>().transform;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !hasTriggered) 
        {
            playerController.FreezeMovement();
            StartCoroutine(ForceLookAtNPC());
            hasTriggered = true;
            gameObject.SetActive(false);
        }
    }

    IEnumerator ForceLookAtNPC() {
        Vector3 directionToNpc = (NPC.transform.position - playerCameraTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNpc);
        float elapsedTime = 0f;
        Quaternion initialRotation = playerCameraTransform.rotation;

        while (elapsedTime < freezeDuration) {
            float t = elapsedTime / freezeDuration;
            playerCameraTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCameraTransform.rotation = targetRotation;
        playerController.UnfreezeMovement();
    }
}
