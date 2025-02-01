using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int trashBagsDisposed = 0;
    public bool toiletTrashPicked = false;
    public bool toiletTrashDisposed = false;
    public bool toiletDetergentAdded = false;

    // Sequence
    [Header("Sequence 0")]
    public GameObject creatureNearWindow; // For enabling/disabling creature near window

    // Sequence 1
    [Header("Sequence 1")]
    public GameObject backLightsController;
    public GameObject backLightsOn;
    public GameObject backLightsOff;
    public GameObject creatureInStorageZero;
    public bool sequenceOneDone = false;

    [Header("Trash Randomness Manager")]
    [SerializeField] private GameObject[] trashGroup0;
    [SerializeField] private GameObject[] trashGroup1;
    [SerializeField] private GameObject[] trashGroup2;

    [Header("Front door")]
    [SerializeField] private GameObject frontDoor;

    private bool hasRandomizedTrash = false;

    private void Start()
    {
        Instance = this;
    }

    public void ActivateNPCOnWindow() {
        creatureNearWindow.SetActive(true);
    }

    public void RunSequenceOne() {
        creatureNearWindow.SetActive(false);
        backLightsController.GetComponent<LightSwitch>().OnInteract();
        creatureInStorageZero.SetActive(true);
        frontDoor.GetComponent<DoorInteraction>().isLocked = false;
        frontDoor.GetComponent<DoorInteraction>().OnInteract();
        sequenceOneDone = true;
    }

    public void EndSequenceOne() {
        if (sequenceOneDone) {
            if (backLightsController.GetComponent<LightSwitch>().lightsAreOn) {
                creatureInStorageZero.SetActive(false);
            }
        }
    }

    public void RandomizeGroups() {
        if (hasRandomizedTrash) return;

        RandomizeGroup(trashGroup0);
        RandomizeGroup(trashGroup1);
        RandomizeGroup(trashGroup2);
        hasRandomizedTrash = true;
    }

    private void RandomizeGroup(GameObject[] group) {
        if (group == null || group.Length == 0) {
            Debug.LogWarning("Trash group is null or empty!");
            return;
        }

        group[0].SetActive(false);

        // First index should be 1 as the second element of the array so it does not activate the first as it's supposed to be inactive.
        int randomIndex = Random.Range(1, group.Length);
        group[randomIndex].SetActive(true);
        Debug.Log(randomIndex);
    }
}
