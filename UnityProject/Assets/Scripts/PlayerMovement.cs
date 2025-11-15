using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 input;

    PlayerController playerController;

    private void Awake() {
        playerController = new PlayerController();
    }

    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable(); 
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        input = playerController.Player.Move.ReadValue<Vector2>();
        input.Normalize();
        if(input.x < 0) {
            rb.transform.localScale = new Vector3(1,1,1);
        }else if(input.x > 0) {
            rb.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate() {
        rb.linearVelocity = input * speed;
    }
}
