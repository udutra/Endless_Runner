using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour {

    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private MainHUD mainHud; //TODO: Remover dependencia do main hud
    [SerializeField] private float reloadGameDelay = 3;
    [SerializeField, Range(0, 5)] private int startGameCountdown = 5;
    [SerializeField] private MusicPlayer musicPlayer;

    private void Awake() {
        SetWaitForStartGameState();
    }

    public void OnGameOver() {
        StartCoroutine(ReloadGameCoroutine());
    }

    private IEnumerator ReloadGameCoroutine() {
        yield return new WaitForSeconds(1);
        musicPlayer.PlayGameOverMusic();
        yield return new WaitForSeconds(reloadGameDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame() {
        StartCoroutine(StartGameCor());
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }

    private IEnumerator StartGameCor() {
        musicPlayer.PlayMainTrackMusic();
        yield return StartCoroutine(mainHud.PlayStartGameCountdown(startGameCountdown));
        playerAnimationController.PlayStartGameAnimation();
    }

    private void SetWaitForStartGameState() {
        player.enabled = false;
        mainHud.ShowStartGameOverlay();
        musicPlayer.PlayStartMenuMusic();
    }
}
