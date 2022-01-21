using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainHUD : MonoBehaviour {
    [SerializeField] private PlayerController player;
    [SerializeField] private GameMode gameMode;

    [Header("Overlays")]
    [SerializeField] private GameObject hudOverlay;
    [SerializeField] private GameObject startGameOverlay;
    [SerializeField] private GameObject pauseOverlay;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI countDownText;

    private void Awake() {
        ShowHudOverlay();
    }

    private void LateUpdate() {
        scoreText.text = $"Score: { player.Score}";
        distanceText.text = $"{ Mathf.RoundToInt(player.TravelledDistance)}m";
    }

    public void PauseGame() { 
        ShowPauseOverlay();
        gameMode.PauseGame();
    }

    public void ResumeGame() {
        ShowHudOverlay();
        gameMode.ResumeGame();
    }

    public void ShowHudOverlay() {
        startGameOverlay.SetActive(false);
        pauseOverlay.SetActive(false);
        hudOverlay.SetActive(true);
    }

    private void ShowPauseOverlay() {
        startGameOverlay.SetActive(false);
        hudOverlay.SetActive(false);
        pauseOverlay.SetActive(true);
        
    }

    public void StartGame() {
        gameMode.StartGame();
    }

    public void ShowStartGameOverlay() {
        hudOverlay.SetActive(false);
        pauseOverlay.SetActive(false);
        startGameOverlay.SetActive(true);
    }

    public IEnumerator PlayStartGameCountdown(int countdownSeconds) {
        ShowHudOverlay();
        countDownText.gameObject.SetActive(false);

        if(countdownSeconds == 0) {
            yield break;
        }

        //Tempo desde que começou o jogo
        float timeToStart = Time.time + countdownSeconds;
        yield return null;

        countDownText.gameObject.SetActive(true);
        while (Time.time <= timeToStart) {
            float remainingTime = timeToStart - Time.time;
            int remainingTimeInt = Mathf.FloorToInt(remainingTime);
            countDownText.text = (remainingTimeInt +1 ).ToString();
            float percent = remainingTime - remainingTimeInt;
            countDownText.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);
            yield return null;
        }
        countDownText.gameObject.SetActive(false);
    }
}