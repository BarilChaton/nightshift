using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LightSwitch : InteractableObject {

    [Header("Affecting objects")]
    [SerializeField] private GameObject lightsOn;
    [SerializeField] private GameObject lightsOff;
    [SerializeField] private GameObject lightSwitchOn;
    [SerializeField] private GameObject lightSwitchOff;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private bool isSequenceAffecting = false;

    [SerializeField] public bool lightsAreOn = true;
    [SerializeField] private GameObject interactorText;

    [SerializeField] private string turnOnLightText = "Test";
    [SerializeField] private string turnOffLightText = "Test";

    [SerializeField] private AudioClip lightSwitchSound;
    private AudioSource lightSwitchSource;

    public override void OnFocus() {
        LookingAt();
    }

    public override void OnInteract() {
        if (lightsAreOn) {
            TurnOffLights();
        } else {
            TurnOnLights();
        }
    }

    public override void OnLoseFocus() {
        TextMeshProUGUI textComponent = interactorText.GetComponent<TextMeshProUGUI>();
        textComponent.text = "";
    }

    private void Start() {
        lightSwitchSource = GetComponent<AudioSource>();
    }

    private void LookingAt() {
        TextMeshProUGUI textComponent = interactorText.GetComponent<TextMeshProUGUI>();
        string textToDisplay = lightsAreOn ? turnOffLightText : turnOnLightText;
        textComponent.text = textToDisplay;
    }

    private void TurnOnLights() {
        lightsOn.SetActive(true);
        lightsOff.SetActive(false);

        lightSwitchOn.SetActive(false);
        lightSwitchOff.SetActive(true);

        lightSwitchSource.PlayOneShot(lightSwitchSound);

        lightsAreOn = true;

        if (isSequenceAffecting) {
            gameManager.GetComponent<GameManager>().EndSequenceOne();
            isSequenceAffecting = false;
        }
    }    
    
    public void TurnOffLights() {
        lightsOff.SetActive(true);
        lightsOn.SetActive(false);

        lightSwitchOff.SetActive(false);
        lightSwitchOn.SetActive(true);

        lightSwitchSource.PlayOneShot(lightSwitchSound);

        lightsAreOn = false;
    }
}
