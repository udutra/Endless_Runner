using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObstacleDecoration : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] private AudioClip collisionAudio;
    [SerializeField] private Animation collisionAnimation;
    private AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;

    public void PlayCollisionFeedback() {
        AudioUtility.PlayAudioSFX(AudioSource, collisionAudio);
        if (collisionAnimation != null) {
            collisionAnimation.Play();
        }
    }
}