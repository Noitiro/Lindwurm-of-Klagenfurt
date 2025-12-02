using UnityEngine;

public class PlayerHead : MonoBehaviour {
    [Header("Ustawienia")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float maxAngle = 45f; 
    [SerializeField] private float minAngle = -45f; 
    private PlayerController playerController;
    private float currentAngle = 0f; 

    private void Awake() {
        playerController = new PlayerController();
    }

    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable();
    }

    void Update() {
        MoveHead();
    }

    private void MoveHead() {
        float input = playerController.Player.MoveHead.ReadValue<Vector2>().y;

        if (input == 0) return;

        float rotationAmount = -input * rotationSpeed * Time.deltaTime;

        currentAngle += rotationAmount;

        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}