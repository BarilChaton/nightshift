using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotScare : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip stingSound;
    private SkinnedMeshRenderer NPCRender;
    private bool hasPlayed = false;


    void Awake() {
        NPCRender = GetComponent<SkinnedMeshRenderer>();
    }

    private void OnBecameVisible() {
        if (!hasPlayed) {
            AudioSource playerAudioSource = player.GetComponent<AudioSource>();
            playerAudioSource.PlayOneShot(stingSound);
            hasPlayed = true;
        }
    }
}
