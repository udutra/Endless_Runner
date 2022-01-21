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

    private void Awake() {
        player.enabled = false;
        mainHud.ShowStartGameOverlay();
    }

    public void OnGameOver() {
        StartCoroutine(ReloadGameCoroutine());
    }

    private IEnumerator ReloadGameCoroutine() {
        //esperar uma frame
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
        yield return StartCoroutine(mainHud.PlayStartGameCountdown(startGameCountdown));
        playerAnimationController.PlayStartGameAnimation();
    }
}
