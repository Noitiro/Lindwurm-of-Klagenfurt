using UnityEngine;
using UnityEngine.UI;

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
    private Animator anim;

    [SerializeField] Image stamina;
    public void UpgradeSpeed(float percent) {
        // MIKO£AJ ZRÓB TO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //  Speed *= (1f + percent);
        //  Sprint *= (1f + percent);
        // 

        Debug.Log("Szybkoœæ gracza zwiêkszona!");
    }
    private void Awake() {
        playerController = new PlayerController();
        playerManager = gameObject.AddComponent<PlayerManager>();
        anim = GetComponent<Animator>();
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
            anim.SetBool("sprint", isSprinting);
        };
        playerController.Player.Sprint.canceled += context => {
            isSprinting = false;
            anim.SetBool("sprint", isSprinting);
        };
    }

    void Update() {
        input = playerController.Player.Move.ReadValue<Vector2>();
        input.Normalize();

        if (input.x < 0 || input.x > 0) {
            anim.SetBool("isWalk", true);
        }

        anim.SetFloat("InputX", input.x);
        anim.SetFloat("InputY", input.y);

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

        stamina.fillAmount = currentEnergy / 100f;

    }

    void FixedUpdate() {
        rb.linearVelocity = input * playerManager.Speed;
    }
}
