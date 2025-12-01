using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] Image staminaBar;

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

    private Transform flipPlayer;
    private Vector3 flipPlayerRight;
    private Vector3 flipPlayerLeft;

    private void Awake() {
        playerController = new PlayerController();
    }
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = normalSpeed;

        flipPlayer = gameObject.transform;
        flipPlayerLeft = new Vector3(flipPlayer.localScale.x, flipPlayer.localScale.y, flipPlayer.localScale.z);
        flipPlayerRight = new Vector3(-flipPlayer.localScale.x, flipPlayer.localScale.y, flipPlayer.localScale.z);

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
            flipPlayer.localScale = flipPlayerRight;
        }else if(moveInput.x < 0) {
            flipPlayer.localScale = flipPlayerLeft;
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
