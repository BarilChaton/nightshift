using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialEncounter : MonoBehaviour
{
    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject player;
    [SerializeField] private float freezeDuration = 7f;
    [SerializeField] private float lookSpeed = 2f;
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
            hasTriggered = true;
            playerController.FreezeMovement();
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
