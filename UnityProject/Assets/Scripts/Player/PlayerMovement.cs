using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Vector2 input;

    PlayerController playerController;
    PlayerManager playerManager;

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
            playerManager.Speed = playerManager.SprintSpeed;
            Debug.Log("Speed: " + playerManager.Speed);
        };
        playerController.Player.Sprint.canceled += context => {
            playerManager.Speed = playerManager.NormalSpeed;
            Debug.Log("Speed: " + playerManager.Speed);
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
    }

    void FixedUpdate() {
        rb.linearVelocity = input * playerManager.Speed;
    }
}
