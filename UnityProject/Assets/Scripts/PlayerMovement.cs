using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 input;

    InputSystem_Actions inputSystemActions;

    private void Awake() {
        inputSystemActions = new InputSystem_Actions();
    }

    private void OnEnable() {
        inputSystemActions.Enable();
    }

    private void OnDisable() {
        inputSystemActions.Disable(); 
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        input = inputSystemActions.Player.Move.ReadValue<Vector2>();
        input.Normalize();
        if(input.x < 0) {
            rb.transform.localScale = new Vector3(5,5,5);
        }else if(input.x > 0) {
            rb.transform.localScale = new Vector3(-5, 5, 5);
        }
    }

    void FixedUpdate() {
        rb.linearVelocity = input * speed;
    }
}
