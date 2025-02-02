using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private string dialogueText = "Why are the lights out?";
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            TextMeshProUGUI textComponent = dialogueUI.GetComponent<TextMeshProUGUI>();
            textComponent.text = dialogueText;
            gameManager.DisableDialogue(5f);
            gameObject.SetActive(false);
        }
    }
}
