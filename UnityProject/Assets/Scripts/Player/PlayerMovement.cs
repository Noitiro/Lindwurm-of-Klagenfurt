using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

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
    }
    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable();
    }

    private void Update() {
        Move();
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

        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);
    }

    public void UpgradeSpeed(float percent) {
        moveSpeed *= (1f + percent);
        moveSpeed *= (1f + percent);

        Debug.Log("Szybkoœæ gracza zwiêkszona!");
    }
}
