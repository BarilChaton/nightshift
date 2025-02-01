using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostItPickup : InteractableObject {
    [SerializeField] private GameManager gameManager;

    public override void OnFocus() {

    }

    public override void OnInteract() {
        gameObject.SetActive(false);
        gameManager.stickyNotesPicked = true;

    }

    public override void OnLoseFocus() {

    }
}
