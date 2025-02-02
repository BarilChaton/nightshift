using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostItPickup : InteractableObject {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject interactorText;
    [SerializeField] private string textToDisplay = "Test";

    public override void OnFocus() {
        TextMeshProUGUI textComponent = interactorText.GetComponent<TextMeshProUGUI>();
        textComponent.text = textToDisplay;
    }

    public override void OnInteract() {
        gameObject.SetActive(false);
        gameManager.stickyNotesPicked = true;

    }

    public override void OnLoseFocus() {
        TextMeshProUGUI textComponent = interactorText.GetComponent<TextMeshProUGUI>();
        textComponent.text = "";
    }
}
