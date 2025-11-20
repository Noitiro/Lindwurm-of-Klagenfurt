using UnityEngine;
using System.Collections;

public abstract class BaseAttack : MonoBehaviour {
    [Header("Ustawienia Bazowe")]
    [SerializeField] protected float attackCooldown = 3f;
    [SerializeField] protected float animationLockTime = 0.5f;
    [SerializeField] protected float damageAmount = 100f;

    [Header("Konfiguracja Obszaru")]
    [SerializeField] protected Transform attackTransform;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] protected LayerMask attackableLayer;

    [Header("Efekty")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected string animationTrigger;

    public float TotalCooldown => attackCooldown;
    public float CurrentCooldown { get; private set; }

    protected Animator anim;
    protected AudioSource audioSource;

    protected virtual void Awake() {
        anim = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
        CurrentCooldown = 0f;

        if (animationLockTime > attackCooldown) {
            Debug.LogWarning($"[{gameObject.name}] Czas animacji jest d³u¿szy ni¿ cooldown!");
        }
    }

    protected virtual void Update() {
        if (CurrentCooldown > 0) {
            CurrentCooldown -= Time.deltaTime;
        }
        else if (CurrentCooldown < 0) {
            CurrentCooldown = 0;
        }
    }

    public bool IsReady() {
        return CurrentCooldown <= 0;
    }

    public void ExecuteAttack(AttackSelector selector) {
        if (!IsReady()) return;

        CurrentCooldown = attackCooldown;
        StartCoroutine(AttackCoroutine(selector));
    }

    private IEnumerator AttackCoroutine(AttackSelector selector) {
        if (!string.IsNullOrEmpty(animationTrigger)) {
            anim.SetTrigger(animationTrigger);
        }

        if (attackSound != null && audioSource != null) {
            audioSource.PlayOneShot(attackSound);
        }

        PerformAttackLogic();

        yield return new WaitForSeconds(animationLockTime);

        selector.SetState(AttackSelector.PlayerState.Idle);
    }

    protected abstract void PerformAttackLogic();

    protected virtual void OnDrawGizmosSelected() {
        if (attackTransform != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}