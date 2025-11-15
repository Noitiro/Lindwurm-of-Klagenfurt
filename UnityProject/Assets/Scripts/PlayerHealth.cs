using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount) {
        currentHealth -= damageAmount;
        Debug.Log("HP Gracza: " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log("Gracz zginął!");
        gameObject.SetActive(false);
    }
}