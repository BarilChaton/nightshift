using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public bool stickyNotesPicked = false;
    public static GameManager Instance;
    public int trashBagsDisposed = 0;
    public bool toiletTrashPicked = false;
    public bool toiletTrashDisposed = false;
    public bool toiletDetergentAdded = false;
    public bool allTrashTakenOut = false;

    private InputAction objectivesAction;
    [SerializeField] private InputActionAsset PlayerActions;
    [SerializeField] private GameObject objectivesUI;
    [SerializeField] private GameObject threeTrashbagsObjective = null;

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

    private void Awake() {
        objectivesAction = PlayerActions.FindActionMap("Player").FindAction("Objectives");
    }

    private void Start() {
        Instance = this;
    }

    private void OnEnable() {
        objectivesAction.Enable();
    }

    private void OnDisable() {
        objectivesAction.Disable();
    }

    private void Update() {
        bool objectivesPressed = objectivesAction.WasPressedThisFrame();
        if (stickyNotesPicked && objectivesPressed) {
            ToggleObjectives();
        }

        if ((trashBagsDisposed == 3 && !toiletTrashDisposed) || (trashBagsDisposed == 4 && toiletTrashDisposed)) {
            threeTrashbagsObjective.gameObject.SetActive(true);
            allTrashTakenOut = true;
        }
    }

    private void ToggleObjectives() {
        if (objectivesUI.activeSelf) {
            objectivesUI.SetActive(false);
        } else {
            objectivesUI.SetActive(true);
        }
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
        if (allTrashTakenOut) return;

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
    }
}
