using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Vector2 input;

    PlayerController playerController;
    PlayerManager playerManager;

    private float maxEnergy = 100f;
    public float currentEnergy = 100f;
    private float sprintCost = 20f;
    private float regenRate = 10f;
    private bool isSprinting = false;

    private void Awake() {
        playerController = new PlayerController();
        playerManager = new PlayerManager();
    }

    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable(); 
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        playerController.Player.Sprint.performed += context => {
            isSprinting = true;
        };
        playerController.Player.Sprint.canceled += context => {
            isSprinting = false;
        };
    }

    void Update() {
        input = playerController.Player.Move.ReadValue<Vector2>();
        input.Normalize();

        if (input.x < 0) {
            rb.transform.localScale = new Vector3(1.6f,1.6f,1.6f);
        }else if(input.x > 0) {
            rb.transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        }

        if (isSprinting && currentEnergy > 0f) {
            playerManager.Speed = playerManager.SprintSpeed;
            currentEnergy -= sprintCost * Time.deltaTime;

            if (currentEnergy <= 0f) {
                currentEnergy = 0f;
                isSprinting = false;
            }
        }
        else {
            playerManager.Speed = playerManager.NormalSpeed;
            currentEnergy += regenRate * Time.deltaTime;

            if (currentEnergy > maxEnergy)
                currentEnergy = maxEnergy;
        }

    }

    void FixedUpdate() {
        rb.linearVelocity = input * playerManager.Speed;
    }
}
