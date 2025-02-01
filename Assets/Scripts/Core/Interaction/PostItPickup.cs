using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostItPickup : InteractableObject {

    public override void OnFocus() {

    }

    public override void OnInteract() {
        gameObject.SetActive(false);
        GameManager.stickyNotesPicked = true;

    }

    public override void OnLoseFocus() {

    }
}
