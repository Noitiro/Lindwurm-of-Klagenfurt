using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static AttackSelector;

public abstract class BaseAttack : MonoBehaviour {
    [Header("Ustawienia Bazowe")]
    [SerializeField] protected float attackCooldown = 3f;
    [SerializeField] protected float animationLockTime = 0.5f;
    [SerializeField] protected float damageAmount = 100f;

    [Header("Konfiguracja Obszaru")]
    [SerializeField] protected Collider2D attackHitbox;
    [SerializeField] protected LayerMask attackableLayer;

    [Header("Efekty ataku")]
    [Tooltip("Animator na obiekcie")]
    [SerializeField] protected Animator effectAnimator;
    [Tooltip("Nazwa triggera")]
    [SerializeField] protected string effectAnimTrigger;

    [Header("Efekty")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected string animationTrigger;
    [Header("Modyfikatory Obra¿eñ")]
    [Tooltip("Rozrzut obra¿eñ w %. Np. 0.1 oznacza +/- 10% (90-110)")]
    [SerializeField] protected float damageVariance = 0.1f;

    [Tooltip("Szansa na trafienie krytyczne (0-1). 0.2 = 20%")]
    [SerializeField] protected float critChance = 0.1f;

    [Tooltip("Mno¿nik obra¿eñ przy krytyku. 2.0 = podwójne obra¿enia")]
    [SerializeField] protected float critMultiplier = 2.0f;

    [Header("Typ Ataku (¯ywio³)")]
    [SerializeField] protected EnemyType strongAgainst = EnemyType.Normal;
    private Light2D aimVisuals;
    [Header("Trzêsienie")]
    [SerializeField] protected GameObject hitEffectPrefab; 
    [SerializeField] protected float force = 0.01f; 
    [SerializeField] protected float shakeDuration = 0.15f;
    [Tooltip("Si³a odrzutu. 0 = brak, 5 = œredni, 10 = mocny")]
    [SerializeField] protected float knockbackForce = 0f;
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

            aimVisuals = attackHitbox.GetComponent<Light2D>();

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

        if (!string.IsNullOrEmpty(animationTrigger) && anim != null) {
            anim.SetTrigger(animationTrigger);
        }

        if (attackSound != null && audioSource != null) {
            audioSource.PlayOneShot(attackSound);
        }
        if (effectAnimator != null && !string.IsNullOrEmpty(effectAnimTrigger)) {
            effectAnimator.gameObject.SetActive(true); 
            effectAnimator.SetTrigger(effectAnimTrigger);
        }

        PerformAttackLogic();

        yield return new WaitForSeconds(animationLockTime);
        if (aimVisuals != null) {
            aimVisuals.enabled = true;
        }

        selector.SetState(AttackSelector.PlayerState.Idle);
    }
    protected List<IDamageable> GetUniqueTargets(List<Collider2D> rawHits) {
        List<IDamageable> uniqueTargets = new List<IDamageable>();

        foreach (var hit in rawHits) {
            IDamageable target = hit.GetComponentInParent<IDamageable>();

            if (target != null) {
                if (!uniqueTargets.Contains(target)) {
                    uniqueTargets.Add(target);
                }
            }
        }

        return uniqueTargets;
    }
    protected float CalculateDamage(BaseEnemyHealth targetEnemy) {
        float finalDamage = damageAmount;

        float varianceModifier = Random.Range(1f - damageVariance, 1f + damageVariance);
        finalDamage *= varianceModifier;

        bool isCrit = Random.value < critChance; 
        if (isCrit) {
            finalDamage *= critMultiplier;
            Debug.Log("<color=red>KRYTYK!</color>");
        }

        if (targetEnemy != null) {
            if (strongAgainst == EnemyType.Ice && targetEnemy.enemyType == EnemyType.Ice) {
                finalDamage *= 2f; 
                Debug.Log("<color=orange>Super Efektywne!</color>");
            }

            if (strongAgainst == EnemyType.Normal && targetEnemy.enemyType == EnemyType.Armored) {
                finalDamage *= 0.5f;
                Debug.Log("Pancerz zablokowa³ czêœæ obra¿eñ.");
            }
        }

        return finalDamage;
    }
    protected void ApplyHitFeedback(GameObject targetObject) {
        if (hitEffectPrefab != null) {
            Instantiate(hitEffectPrefab, targetObject.transform.position, Quaternion.identity);
        }

        if (CameraShake.Instance != null) {


            CameraShake.Instance.Shake(shakeDuration, force);
        }
        if (HitStop.Instance != null) {
            HitStop.Instance.Stop(0.05f);
        }
        if (knockbackForce > 0) {
            EnemyKnockback knockbackScript = targetObject.GetComponent<EnemyKnockback>();
            if (knockbackScript != null) {
                knockbackScript.ApplyKnockback(transform, knockbackForce);
            }
        }
    }

    protected abstract void PerformAttackLogic();

    public void ApplyUpgrade(UpgradeCardSO.UpgradeType type, float value) {
        switch (type) {
            case UpgradeCardSO.UpgradeType.Damage: UpgradeDamage(value); break;
            case UpgradeCardSO.UpgradeType.AreaSize: UpgradeArea(value); break;
            case UpgradeCardSO.UpgradeType.Knockback: UpgradeKnockback(value); break;
            case UpgradeCardSO.UpgradeType.CritChance: UpgradeCrit(value); break;
        }
    }
    public void UpgradeDamage(float percent) {
        damageAmount += damageAmount * percent;
        Debug.Log($"{gameObject.name}: DMG zwiêkszony do {damageAmount}");
    }
    public void UpgradeArea(float percent) {
        if (attackHitbox != null) {
            Vector3 currentScale = attackHitbox.transform.localScale;
            attackHitbox.transform.localScale = currentScale * (1f + percent);

            Debug.Log($"{gameObject.name}: Wielkoœæ zwiêkszona!");
        }
    }
    public void UpgradeKnockback(float amount) {
        knockbackForce += amount;
        Debug.Log($"{gameObject.name}: Odrzut zwiêkszony do {knockbackForce}");
    }
    public void UpgradeCrit(float amount) {
        critChance += amount;
        if (critChance > 1f) critChance = 1f; 
        Debug.Log($"{gameObject.name}: Szansa na kryta: {critChance}");
    }
}