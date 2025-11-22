using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseAttack : MonoBehaviour {
    [Header("Ustawienia Bazowe")]
    [SerializeField] protected float attackCooldown = 3f;
    [SerializeField] protected float animationLockTime = 0.5f;
    [SerializeField] protected float damageAmount = 100f;

    [Header("Konfiguracja Obszaru")]
    [SerializeField] protected Collider2D attackHitbox;
    [SerializeField] protected LayerMask attackableLayer;

    [Header("Efekty")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected string animationTrigger;

    private SpriteRenderer aimVisuals;

    public float TotalCooldown => attackCooldown;
    public float CurrentCooldown { get; private set; }

    protected Animator anim;
    protected AudioSource audioSource;
    protected ContactFilter2D contactFilter;

    protected virtual void Awake() {
        anim = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
        CurrentCooldown = 0f;

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(attackableLayer);
        contactFilter.useTriggers = true;

        if (attackHitbox != null) {
            attackHitbox.isTrigger = true;

            aimVisuals = attackHitbox.GetComponent<SpriteRenderer>();

            if (aimVisuals != null) {
                aimVisuals.enabled = false;
                Color c = aimVisuals.color;
                c.a = 0.3f; 
                aimVisuals.color = c;
            }
        }
    }

    protected virtual void Update() {
        if (CurrentCooldown > 0) CurrentCooldown -= Time.deltaTime;
        else if (CurrentCooldown < 0) CurrentCooldown = 0;
    }

    public bool IsReady() => CurrentCooldown <= 0;

    public void TogglePreview(bool show) {
        if (aimVisuals != null) {
            aimVisuals.enabled = show;
        }
    }

    public void ExecuteAttack(AttackSelector selector) {
        if (!IsReady()) return;
        CurrentCooldown = attackCooldown;
        StartCoroutine(AttackCoroutine(selector));
    }

    private IEnumerator AttackCoroutine(AttackSelector selector) {

        if (aimVisuals != null) {
            aimVisuals.enabled = false;
        }

        if (!string.IsNullOrEmpty(animationTrigger)) anim.SetTrigger(animationTrigger);
        if (attackSound != null && audioSource != null) audioSource.PlayOneShot(attackSound);

        PerformAttackLogic();

        yield return new WaitForSeconds(animationLockTime);

        if (aimVisuals != null) {
            aimVisuals.enabled = true;
        }

        selector.SetState(AttackSelector.PlayerState.Idle);
    }

    protected abstract void PerformAttackLogic();
}