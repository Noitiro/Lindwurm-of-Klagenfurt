using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    PlayerController playerController;
    private Animator anim;

    private void Awake() {
        playerController = new PlayerController();
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable();
    }

    private void Start() {
        playerController.Player.Attack.performed += context => {
            TakeDamage(1);
        };

    }

    public void TakeDamage(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log("Player damage: " + damage);
        if (currentHealth > 0) {
            anim.SetTrigger("hurt");
        }else {
            anim.SetTrigger("die");
        }
        //s
    }
}
