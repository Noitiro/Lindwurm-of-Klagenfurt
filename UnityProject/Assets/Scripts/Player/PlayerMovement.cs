using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] Image staminaBar;
    [SerializeField] GameObject parentHitbox;

    private float moveSpeed;
    private bool canSprint;
    private bool pressSprint;
    private float maxEnergy = 100f;
    private float currentEnergy = 100f;
    private float sprintCost = 20f;
    private float regenRate = 10f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerController playerController;

    private void Awake() {
        playerController = new PlayerController();
    }
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = normalSpeed;
    }
    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable();
    }

    private void Update() {
        Move();
        Sprint();
        rb.linearVelocity = moveInput * moveSpeed;
    }

    //---------------------------------------------

    private void Move() {
        playerController.Player.Move.performed += context => {
            anim.SetBool("isWalk", true);
        };

        playerController.Player.Move.canceled += context => {
            anim.SetBool("isWalk", false);
            anim.SetFloat("LastInputX", moveInput.x);
            anim.SetFloat("LastInputY", moveInput.y);
        };

        moveInput = playerController.Player.Move.ReadValue<Vector2>();
        moveInput.Normalize();

        if(moveInput.x > 0) {
            parentHitbox.transform.localScale = new Vector3(-1, 1, 1);
        }else if(moveInput.x < 0) {
            parentHitbox.transform.localScale = new Vector3(1, 1, 1);
        }

        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);
    }

    private void Sprint() {
        playerController.Player.Sprint.performed += context => {
            pressSprint = true;
        };

        playerController.Player.Sprint.canceled += context => {
            anim.SetBool("isSprint", false);
            pressSprint = false;
        };

        if (anim.GetBool("isWalk") && pressSprint && canSprint && currentEnergy > 0f) {
            moveSpeed = sprintSpeed;
            anim.SetBool("isSprint", true);

            currentEnergy = Mathf.Clamp(currentEnergy - sprintCost * Time.deltaTime, 0, maxEnergy);
            if (currentEnergy == 0f) {
                canSprint = false;
                anim.SetBool("isSprint", false);
                moveSpeed = normalSpeed;
            }
        } else {
            currentEnergy = Mathf.Clamp(currentEnergy + regenRate * Time.deltaTime, 0, maxEnergy);
            if (currentEnergy > 10f) {
                canSprint = true;
            }
        }

        staminaBar.fillAmount = currentEnergy / 100f;

    }

    public void UpgradeSpeed(float percentNormalSpeed, float percentSprintSpeed) {
        moveSpeed *= (1f + percentNormalSpeed);
        normalSpeed *= (1f + percentNormalSpeed);
        sprintSpeed *= (1f + percentSprintSpeed);

        Debug.Log("Szybkoœæ gracza zwiêkszona!");
    }
}
