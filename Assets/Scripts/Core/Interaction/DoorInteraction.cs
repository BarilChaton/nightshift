using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorInteraction : InteractableObject {

    public static DoorInteraction Instance;

    [SerializeField] private float openAngle = -90f;
    [SerializeField] private float closedAngle = 0f;
    [SerializeField] private float openSpeed = 4f;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private bool isInteractable = true;
    [SerializeField] public bool isLocked = false;

    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    private AudioSource audioSource;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        closedRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, closedAngle, 0));
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }


    public override void OnFocus() {

    }

    public override void OnInteract() {
        if (isInteractable) {
            if (!isOpen && !isLocked) {
                StartCoroutine(OpenDoor());
            } else {
                StartCoroutine(CloseDoor());
            }
        }

        isOpen = !isOpen;
    }

    public override void OnLoseFocus() {

    }

    private IEnumerator OpenDoor() {
        audioSource.PlayOneShot(doorOpen);

        while (Quaternion.Angle(transform.rotation, openRotation) > 0.001f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
            isInteractable = false;
            yield return null;
        }

        transform.rotation = openRotation;
        isInteractable = true;
    }

    private IEnumerator CloseDoor() {
        audioSource.PlayOneShot(doorClose);
        while (Quaternion.Angle(transform.rotation, closedRotation) > 0.001f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * openSpeed);
            isInteractable = false;
            yield return null;
        }

        transform.rotation = closedRotation;
        isInteractable = true;
    }
}
