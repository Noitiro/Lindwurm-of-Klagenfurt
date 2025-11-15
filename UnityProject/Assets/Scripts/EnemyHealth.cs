using UnityEngine;

public class EnemiesHealth : MonoBehaviour {
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log("Przeciwnik zginął!");
        Destroy(gameObject);
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }
}
