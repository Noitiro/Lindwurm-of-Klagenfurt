using UnityEngine;

public class EnemiesHealth : MonoBehaviour, IDamageable {
    [SerializeField] private float maxHealth = 100;
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
