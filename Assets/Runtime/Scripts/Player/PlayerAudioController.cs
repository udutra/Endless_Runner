using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound, rollSound;

    private AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;

    private void Play(AudioClip clip) {
        AudioUtility.PlayAudioSFX(AudioSource, clip);
    }

    public void PlayJumpSound() {
        Play(jumpSound);
    }

    public void PlayRollSound() {
        Play(rollSound);
    }
}