using UnityEngine;
using UnityEngine.Windows;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float startingHealth = 200;
    public float currentHealth { get; private set; }
    private Animator anim;
    [SerializeField] private GameObject GameOverScreen;
    private Rigidbody2D rb;

    private void Awake() {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Damage(float damage) {

        if (currentHealth > 0) {
       //     rb.AddForce = new Vector3 (rb.position.x-0.2f, rb.position.y);
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
            Debug.Log("Player damage: " + damage);
            anim.SetTrigger("hurt");
        } 
        else { 
            anim.SetTrigger("die");
            
            GameOverScreen.SetActive(true);
        }
    }
}
