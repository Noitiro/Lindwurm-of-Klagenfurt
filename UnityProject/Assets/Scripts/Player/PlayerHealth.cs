using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable {
    [SerializeField] private float startingHealth = 200;
    public float currentHealth { get; private set; }
    private Animator anim;
    [SerializeField] private Canvas GameOverScreen;
    private Rigidbody2D rb;
    private bool isDead = false;

    private void Awake() {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        GameOverScreen = GetComponentInChildren<Canvas>();
    }

    IEnumerator coldownDie() {
        yield return new WaitForSeconds(0.9f);
        GameOverScreen.gameObject.SetActive(true);
    }

    public void KillPlayer() {
        if (isDead) return;

        isDead = true;
        currentHealth = 0; 
        Debug.Log("Wymuszona œmieræ gracza!");

        anim.SetTrigger("die");
        StartCoroutine(coldownDie());
    }

    public void Damage(float damage) {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log("Player damage: " + damage);

        if (currentHealth > 0) {
            anim.SetTrigger("hurt");
        }
        else {
            KillPlayer();
        }
    }
}