using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth;

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

    public void Die() {
        Debug.Log("Gracz zginął!");
        gameObject.SetActive(false);
    }
}