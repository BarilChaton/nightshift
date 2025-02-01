using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float detectionDistance;

    private Quaternion originalRotation;

    private void Start() {
        originalRotation = transform.rotation;
    }

    private void Update() {
        Vector3 directionToPlayer = transform.position - player.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= detectionDistance) {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
