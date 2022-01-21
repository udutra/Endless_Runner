using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainHUD : MonoBehaviour {
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button testButton;

    private void LateUpdate() {
        scoreText.text = $"Score: { player.Score}";
    }

    public void TestButton() {
        testButton.interactable = false;
        Debug.Log("Clicked!");
    }
}