using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float laneDistanceX = 1.3f;
    private Vector3 initiallPosition;
    
    private float LeftLaneX => initiallPosition.x - laneDistanceX;
    private float RightLaneX => initiallPosition.x + laneDistanceX;
    private float targetPositionX;


    [Header("Jump")]
    private float jumpStartZ;
    
    [SerializeField] private float jumpDistanceZ = 5;
    [SerializeField] private float jumpHeightY = 2;
    [SerializeField] private float jumpLerpSpeed = 10;
    private bool CanJump => !IsJumping;

    public float JumpDuration => jumpDistanceZ / forwardSpeed;
    public bool IsJumping { get; private set; }

    [Header("Rool")]
    private float rollStartZ;
    private bool CanRoll => !IsRolling;
    [SerializeField] private float rollDistanceZ = 5;
    [SerializeField] private Collider regularCollider, rollCollider;
    public float RollDutarion => rollDistanceZ / forwardSpeed;
    public bool IsRolling { get; private set; }
    
    private void Awake() {
        initiallPosition = transform.position;
        StopRoll();
    }

    private void Update() {
        ProcessInput();

        Vector3 position = transform.position;

        position.x = ProcessLaneMovementt();
        position.y = ProcessJump();
        position.z = ProcessFowardMovementt();
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

    private float ProcessLaneMovementt() {
        return Mathf.Lerp(transform.position.x, targetPositionX, Time.deltaTime * horizontalSpeed);
    }

    private float ProcessFowardMovementt() {
        return transform.position.z + forwardSpeed * Time.deltaTime;
    }

    private void StartJump() {
        IsJumping = true;
        jumpStartZ = transform.position.z;
        StopRoll();
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
        float targetPositionY = initiallPosition.y + deltaY;
        return Mathf.Lerp(transform.position.y, targetPositionY, Time.deltaTime * jumpLerpSpeed);
    }
    private void StopJump() {
        IsJumping = false;
    }


    private void StartRoll() {
        rollStartZ = transform.position.z;
        IsRolling = true;
        regularCollider.enabled = false;
        rollCollider.enabled = true;
        StopJump();
    }

    private void ProcessRoll() {
        if (IsRolling) {
            float percent = (transform.position.z - rollStartZ) / rollDistanceZ;
            if (percent >=1) {
                StopRoll();
            }
        }
    }

    private void StopRoll() {
        IsRolling = false;
        regularCollider.enabled = true;
        rollCollider.enabled = false;
    }

    public void Die() {
        forwardSpeed = 0;
        StopRoll();
        StopJump();
    }
}