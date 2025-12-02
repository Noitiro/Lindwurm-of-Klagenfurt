using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform rotateAround;
    private PlayerController playerController;
    private Vector2 moveHeadInput;

    private void Awake() {
        playerController = new PlayerController();
    }

    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable();
    }

    void Update()
    {
        MoveHead();
    }

    private void MoveHead() {
        moveHeadInput = playerController.Player.MoveHead.ReadValue<Vector2>();
        moveHeadInput.Normalize();

        if(moveHeadInput.y > 0 && this.transform.rotation.z > -0.45f) {
            this.transform.RotateAround(rotateAround.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
        } else if (moveHeadInput.y < 0 && this.transform.rotation.z < 0.45f) {
            this.transform.RotateAround(rotateAround.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
