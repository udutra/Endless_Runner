using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerCollision : MonoBehaviour {

    private PlayerController playerController;
    private PlayerAnimationController animationController;
    [SerializeField] private GameController gameController;
    
    private void Awake() {
        playerController = GetComponent<PlayerController>(); ;
        animationController = GetComponent<PlayerAnimationController>();
    }


    private void OnTriggerEnter(Collider other) {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle != null) {
            playerController.Die();
            animationController.Die();
            gameController.OnGameOver();
        }
    }
}
