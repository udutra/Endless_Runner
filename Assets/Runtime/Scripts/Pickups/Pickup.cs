using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour {
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private AudioClip pickupAudio;
    [SerializeField] private GameObject model;

    private void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
    }

    public void OnPickedUp() {
        AudioSource audioSource = GetComponent<AudioSource>();
        AudioUtility.PlayAudioSFX(audioSource, pickupAudio);

        //TODO: Mover toque de audio para um AudioService
        model.SetActive(false);
        Destroy(gameObject, pickupAudio.length);
    }
}
