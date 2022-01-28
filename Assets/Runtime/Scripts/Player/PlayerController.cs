using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector3 initialPosition;
    private float targetPositionX;
    private float rollStartZ;
    private float jumpStartZ;
    private bool isDead;

    [Header("Audio")]
    [SerializeField] private PlayerAudioController audioController;

    [Header("Speed")]
    [SerializeField] private float horizontalSpeed = 15;
    //[SerializeField] private float forwardSpeed = 10;

    [Header("Lane")]
    [SerializeField] private float laneDistanceX = 4;

    [Header("Jump")]
    [SerializeField] private float jumpDistanceZ = 5;
    [SerializeField] private float jumpHeightY = 2;
    [SerializeField] private float jumpLerpSpeed = 10;

    [Header("Roll")]
    [SerializeField] private float rollDistanceZ = 5;
    [SerializeField] private Collider regularCollider;
    [SerializeField] private Collider rollCollider;

    private float LeftLaneX => initialPosition.x - laneDistanceX;
    private float RightLaneX => initialPosition.x + laneDistanceX;
    private bool CanJump => !IsJumping;
    private bool CanRoll => !IsRolling;

    public float JumpDuration => jumpDistanceZ / ForwardSpeed;
    public float RollDuration => rollDistanceZ / ForwardSpeed;
    public float TravelledDistance => transform.position.z - initialPosition.z;
    public float ForwardSpeed { get; set; } = 10;
    public bool IsJumping { get; private set; }
    public bool IsRolling { get; private set; }

    private void Awake() {
        initialPosition = transform.position;
        StopRoll();
    }

    private void Update() {
        if (!isDead) {
            ProcessInput();
        }

        Vector3 position = transform.position;

        position.x = ProcessLaneMovement();
        position.y = ProcessJump();
        position.z = ProcessForwardMovement();
        ProcessRoll();

        transform.position = position;
    }

    private void ProcessInput() {
        if (Input.GetKeyDown(KeyCode.D)) {
            targetPositionX += laneDistanceX;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            targetPositionX -= laneDistanceX;
        }
        if (Input.GetKeyDown(KeyCode.W) && CanJump) {
            StartJump();
        }
        if (Input.GetKeyDown(KeyCode.S) && CanRoll) {
            StartRoll();
        }

        targetPositionX = Mathf.Clamp(targetPositionX, LeftLaneX, RightLaneX);
    }

    private float ProcessLaneMovement() {
        return Mathf.Lerp(transform.position.x, targetPositionX, Time.deltaTime * horizontalSpeed);
    }

    private float ProcessForwardMovement() {
        return transform.position.z + ForwardSpeed * Time.deltaTime;
    }

    private void StartJump() {
        IsJumping = true;
        jumpStartZ = transform.position.z;
        StopRoll();
        audioController.PlayJumpSound();
    }

    private void StopJump() {
        IsJumping = false;
    }

    private float ProcessJump() {
        float deltaY = 0;
        if (IsJumping) {
            float jumpCurrentProgress = transform.position.z - jumpStartZ;
            float jumpPercent = jumpCurrentProgress / jumpDistanceZ;
            if (jumpPercent >= 1) {
                StopJump();
            }
            else {
                deltaY = Mathf.Sin(Mathf.PI * jumpPercent) * jumpHeightY;
            }
        }
        float targetPositionY = initialPosition.y + deltaY;
        return Mathf.Lerp(transform.position.y, targetPositionY, Time.deltaTime * jumpLerpSpeed);
    }

    private void ProcessRoll() {
        if (IsRolling) {
            float percent = (transform.position.z - rollStartZ) / rollDistanceZ;
            if (percent >= 1) {
                StopRoll();
            }
        }
    }

    private void StartRoll() {
        rollStartZ = transform.position.z;
        IsRolling = true;
        regularCollider.enabled = false;
        rollCollider.enabled = true;

        StopJump();

        audioController.PlayRollSound();
    }

    private void StopRoll() {
        IsRolling = false;
        regularCollider.enabled = true;
        rollCollider.enabled = false;
    }

    public void Die() {
        ForwardSpeed = 0;
        horizontalSpeed = 0;
        targetPositionX = transform.position.x;
        isDead = true;
        StopRoll();
        StopJump();
        regularCollider.enabled = false;
        rollCollider.enabled = false;
    }
}