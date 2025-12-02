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

    [Header("Efekty Wizualne")]
    [SerializeField] private float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private bool isBurning = false;
    private bool isFrozen = false;
    private Coroutine burnCoroutine;
    private EnemiesFollowsAI movementScript;

    protected virtual void Awake() {
        CurrentHealth = maxHealth;
        movementScript = GetComponent<EnemiesFollowsAI>();

        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    protected virtual void OnEnable() {
        EnemiesAliveCount++;
        OnEnemyCountChanged?.Invoke(EnemiesAliveCount);
    }

    protected virtual void OnDisable() {
        EnemiesAliveCount--;
        if (EnemiesAliveCount < 0) EnemiesAliveCount = 0;
        OnEnemyCountChanged?.Invoke(EnemiesAliveCount);
    }

    public virtual void Damage(float amount) {
        CurrentHealth -= amount;
        Flash(Color.grey);

        if (damagePopupPrefab != null) {
            ShowDamagePopup(amount);
        }

        if (CurrentHealth <= 0) {
            Die();
        }
    }

    public void Flash(Color flashColor) {
        if (sr == null) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine(flashColor));
    }

    private IEnumerator FlashRoutine(Color targetColor) {
        if (sr != null) sr.color = targetColor;

        yield return new WaitForSeconds(flashDuration);

        if (sr != null) {
            if (isFrozen) sr.color = Color.cyan;
            else if (isBurning) sr.color = new Color(1f, 0.6f, 0.6f);
            else sr.color = originalColor;
        }

        flashCoroutine = null;
    }

    public void ApplyFreeze(float slowFactor, float duration) {
        if (isBurning) {
            if (burnCoroutine != null) StopCoroutine(burnCoroutine);
            isBurning = false;
            Debug.Log("<color=yellow>OGIEÑ ZGASZONY!</color>");
            if (sr != null) sr.color = originalColor;
            return;
        }

        if (movementScript != null) {
            isFrozen = true;
            movementScript.ApplySlow(slowFactor, duration);
            StartCoroutine(ResetFrozenState(duration));
        }
        Flash(Color.cyan);
    }

    private IEnumerator ResetFrozenState(float time) {
        yield return new WaitForSeconds(time);
        isFrozen = false;
        if (sr != null && !isBurning) sr.color = originalColor;
    }

    public void ApplyBurn(float damagePerTick, int ticks, float interval) {
        if (isFrozen) {
            if (movementScript != null) movementScript.RemoveSlow();
            isFrozen = false;
            Debug.Log("<color=yellow>LÓD ROZTOPIONY!</color>");
            if (sr != null) sr.color = originalColor;
            return;
        }

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(BurnCoroutine(damagePerTick, ticks, interval));
    }

    private IEnumerator BurnCoroutine(float dmg, int ticks, float interval) {
        isBurning = true;

        if (sr != null) sr.color = new Color(1f, 0.6f, 0.6f);

        for (int i = 0; i < ticks; i++) {
            yield return new WaitForSeconds(interval);

            CurrentHealth -= dmg;
            if (damagePopupPrefab != null) ShowDamagePopup(dmg);

            Flash(Color.red);

            if (CurrentHealth <= 0) {
                Die();
                yield break;
            }
        }
        isBurning = false;
        if (sr != null && !isFrozen) sr.color = originalColor;
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