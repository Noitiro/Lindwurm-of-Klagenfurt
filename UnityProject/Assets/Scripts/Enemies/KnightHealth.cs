using UnityEngine;

public class KnightHealth : MonoBehaviour, IDamageable {
    private float maxHealth = 200;
    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public void Damage(float amount) {
        currentHealth -= amount;

        Debug.Log("Enemy takes damage: " + amount);

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log("Przeciwnik zginął!");
        Destroy(gameObject);
    }
}
