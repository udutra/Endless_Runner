using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField] private AudioClip startMenuMusic, mainTrackMusic, gameOverMusic;

    private AudioSource AudioSource => audioSource == null ? audioSource = GetComponent<AudioSource>() : audioSource;

    private void Update() {
        if (Input.GetKeyUp(KeyCode.M)) {
            StopMusic();
        }
    }

    private void PlayMusic(AudioClip clip) {
        AudioUtility.PlayMusic(AudioSource, clip);
    }

    private void StopMusic() {
        AudioSource.Stop();
    }

    public void PlayStartMenuMusic() {
        PlayMusic(startMenuMusic);
    }

    public void PlayMainTrackMusic() {
        PlayMusic(mainTrackMusic);
    }

    public void PlayGameOverMusic() {
        PlayMusic(gameOverMusic);
    }
}
