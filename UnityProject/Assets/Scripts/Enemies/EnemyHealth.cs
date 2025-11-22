using UnityEngine;

public class EnemiesHealth : MonoBehaviour, IDamageable {

    [Header("Statystyki")]
    [SerializeField] private float maxHealth = 100;
    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;

    [Header("Efekty UI")]
    [Tooltip("Przeciągnij tutaj prefab DamagePopup")]
    [SerializeField] private GameObject damagePopupPrefab;

    private void Awake() {
        CurrentHealth = maxHealth;
    }

    public void Damage(float amount) {
        CurrentHealth -= amount;
        if (damagePopupPrefab != null) {
            ShowDamagePopup(amount);
        }

        Debug.Log("Enemy takes damage: " + amount);

        if (CurrentHealth <= 0) {
            Die();
        }
    }

    private void ShowDamagePopup(float amount) {
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

        GameObject popup = Instantiate(damagePopupPrefab, transform.position + randomOffset, Quaternion.identity);

        DamagePopup popupScript = popup.GetComponent<DamagePopup>();
        if (popupScript != null) {
            popupScript.Setup(amount);
        }
    }

    private void Die() {
        Debug.Log("Przeciwnik zginął!");
        Destroy(gameObject);
    }
}