using System.Collections;
using UnityEngine;
using System;
using static AttackSelector; 

public class BaseEnemyHealth : MonoBehaviour, IDamageable {
    public static int EnemiesAliveCount = 0;
    public static event Action<int> OnEnemyCountChanged;

    [Header("Statystyki")]
    [SerializeField] protected float maxHealth = 100f;
    [Header("Typ Przeciwnika")]
    [SerializeField] public EnemyType enemyType = EnemyType.Normal;
    [Header("Nagroda")]
    [SerializeField] protected int scoreValue = 1;

    public float CurrentHealth { get; protected set; }
    public float MaxHealth => maxHealth;

    [Header("Efekty UI")]
    [SerializeField] protected GameObject damagePopupPrefab;

    private bool isBurning = false;
    private bool isFrozen = false;
    private Coroutine burnCoroutine;
    private EnemiesFollowsAI movementScript;


    protected virtual void OnEnable() {
        EnemiesAliveCount++;
        OnEnemyCountChanged?.Invoke(EnemiesAliveCount);
    }
    protected virtual void OnDisable() {
        EnemiesAliveCount--;
        if (EnemiesAliveCount < 0) EnemiesAliveCount = 0;
        OnEnemyCountChanged?.Invoke(EnemiesAliveCount);
    }
    protected virtual void Awake() {
        CurrentHealth = maxHealth;
        movementScript = GetComponent<EnemiesFollowsAI>();
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

    public void ApplyFreeze(float slowFactor, float duration) {
        if (isBurning) {
            if (burnCoroutine != null) StopCoroutine(burnCoroutine);
            isBurning = false;
            Debug.Log("<color=yellow>OGIEÑ ZGASZONY!</color>");
            return; 
        }
        if (movementScript != null) {
            isFrozen = true;
            movementScript.ApplySlow(slowFactor, duration);
    
            StartCoroutine(ResetFrozenState(duration));
        }
    }

    private IEnumerator ResetFrozenState(float time) {
        yield return new WaitForSeconds(time);
        isFrozen = false;
    }

    public void ApplyBurn(float damagePerTick, int ticks, float interval) {
        if (isFrozen) {
            if (movementScript != null) movementScript.RemoveSlow();
            isFrozen = false;
            Debug.Log("<color=yellow>LÓD ROZTOPIONY!</color>");
            return; 
        }

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(BurnCoroutine(damagePerTick, ticks, interval));
    }

    private IEnumerator BurnCoroutine(float dmg, int ticks, float interval) {
        isBurning = true;
        for (int i = 0; i < ticks; i++) {
            yield return new WaitForSeconds(interval);

            CurrentHealth -= dmg;
            if (damagePopupPrefab != null) ShowDamagePopup(dmg);

            if (CurrentHealth <= 0) {
                Die();
                yield break;
            }
        }
        isBurning = false;
    }

    protected virtual void ShowDamagePopup(float amount) {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0.5f, 0);
        GameObject popup = Instantiate(damagePopupPrefab, transform.position + randomOffset, Quaternion.identity);
        popup.GetComponent<DamagePopup>()?.Setup(amount);
    }

    protected virtual void Die() {
        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.AddScore(scoreValue);
        }
        Destroy(gameObject);
    }
}