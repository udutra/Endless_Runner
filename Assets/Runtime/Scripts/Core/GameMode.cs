using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour {

    private float startGameTime;
    [SerializeField] private bool isGameRunning = false;

    [Header("Player")]
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerAnimationController playerAnimationController;

    [Header("UI")]
    [SerializeField] private MainHUD mainHud;
    [SerializeField] private MusicPlayer musicPlayer;

    [Header("Gameplay")]
    [SerializeField] private int startPlayerSpeed = 10;
    [SerializeField] private int maxPlayerSpeed = 17;
    [SerializeField] private float timeToMaxSpeedSeconds = 300f;
    [SerializeField] private float reloadGameDelay = 3;
    [SerializeField, Range(0, 5)] private int startGameCountdown = 5;

    [Header("Socre")]
    [SerializeField] private int baseScoreMultiplier = 1;
    [SerializeField] private float score;
    public int Score => Mathf.RoundToInt(score);

    private void Awake() {
        SetWaitForStartGameState();
    }

    private void Update() {
        if (isGameRunning) {
            
            float timePercent = (Time.time - startGameTime) / timeToMaxSpeedSeconds;
            player.ForwardSpeed = Mathf.Lerp(startPlayerSpeed, maxPlayerSpeed, timePercent);
            float extraScoreMultiplier = 1 + timePercent;
            Debug.Log("update");
            score += baseScoreMultiplier * extraScoreMultiplier * player.ForwardSpeed * Time.deltaTime;
        }
    }

    public void OnGameOver() {
        isGameRunning = false;
        player.ForwardSpeed = 0;
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
        Debug.Log("Entrou");
        musicPlayer.PlayMainTrackMusic();
        yield return StartCoroutine(mainHud.PlayStartGameCountdown(startGameCountdown));
        yield return StartCoroutine(playerAnimationController.PlayStartGameAnimation());

        player.enabled = true;
        player.ForwardSpeed = startPlayerSpeed;
        startGameTime = Time.time;
        isGameRunning = true;
    }

    private void SetWaitForStartGameState() {
        player.enabled = false;
        isGameRunning = false;
        mainHud.ShowStartGameOverlay();
        musicPlayer.PlayStartMenuMusic();
    }
}
