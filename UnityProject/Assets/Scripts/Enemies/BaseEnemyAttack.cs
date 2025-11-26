using UnityEngine;
using static AttackSelector;

public class BaseEnemyHealth : MonoBehaviour, IDamageable {
    [Header("Statystyki")]
    [SerializeField] protected float maxHealth = 100f;
    [Header("Typ Przeciwnika")]
    [SerializeField] public EnemyType enemyType = EnemyType.Normal;
    [Header("Nagroda")]
    [SerializeField] protected int scoreValue = 1;
    public float CurrentHealth { get; protected set; }
    public float MaxHealth => maxHealth;

    [Header("Efekty UI")]
    [Tooltip("Przeci¹gnij tutaj prefab DamagePopup")]
    [SerializeField] protected GameObject damagePopupPrefab;

    protected virtual void Awake() {
        CurrentHealth = maxHealth;
    }

    public virtual void Damage(float amount) {
        CurrentHealth -= amount;

        if (damagePopupPrefab != null) {
            ShowDamagePopup(amount);
        }

        Debug.Log($"{gameObject.name} otrzyma³ {amount} obra¿eñ. HP: {CurrentHealth}");

        if (CurrentHealth <= 0) {
            Die();
        }
    }

    protected virtual void ShowDamagePopup(float amount) {
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);
        GameObject popup = Instantiate(damagePopupPrefab, transform.position + randomOffset, Quaternion.identity);

        DamagePopup popupScript = popup.GetComponent<DamagePopup>();
        if (popupScript != null) {
            popupScript.Setup(amount);
        }
    }

    protected virtual void Die() {
        Debug.Log($"{gameObject.name} zgin¹³!");
        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.AddScore(scoreValue);
        }
        Destroy(gameObject);
    }
}