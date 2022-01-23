using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHUDAudioController : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField] private AudioClip buttonAudio, countdownAudio, countdownFinishedAudio;

    private AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;

    private void Play(AudioClip clip) {
        AudioUtility.PlayAudioSFX(AudioSource, clip);
    }

    public void PlayButtonAudio() {
        Play(buttonAudio);
    }

    public void PlayCountdownAudio() {
        Play(countdownAudio);
    }

    public void PlayCountdownFinishhedAudio() {
        Play(countdownFinishedAudio);
    }
}
