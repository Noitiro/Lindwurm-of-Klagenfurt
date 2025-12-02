using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHead : MonoBehaviour {
    [Header("Ustawienia")]
    [Tooltip("Prêdkoœæ obrotu (dla klawiatury i myszki)")]
    [SerializeField] private float rotationSpeed = 150f;

    [Tooltip("Maksymalny k¹t w górê (np. 45 stopni)")]
    [SerializeField] private float maxAngle = 45f;

    [Tooltip("Maksymalny k¹t w dó³ (np. -45 stopni)")]
    [SerializeField] private float minAngle = -45f;

    private PlayerController playerController;
    private float currentAngle = 0f; // Pamiêæ aktualnego k¹ta

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
        Vector2 input = playerController.Player.MoveHead.ReadValue<Vector2>();

        if (Mathf.Abs(input.y) < 0.01f) return;

        float rotationAmount = input.y * rotationSpeed * Time.deltaTime;

        currentAngle -= rotationAmount;

        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}