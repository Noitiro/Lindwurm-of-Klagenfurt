using UnityEngine;
using System.Collections;

public class ClawAttack : MonoBehaviour {
    [Header("Ustawienia Ataku")]
    [Tooltip("Ca�kowity czas odnowienia (np. 3 sekundy)")]
    [SerializeField] private float attackCooldown = 3f;
    public float TotalCooldown { get { return attackCooldown; } }

    [Tooltip("Czas trwania animacji ataku (np. 0.5 sekundy)")]
    [SerializeField] private float animationLockTime = 0.5f;

    [Tooltip("Ilo�� obra�e�")]
    [SerializeField] private float damageAmount = 100f;

    [Tooltip("Punkt, z kt�rego wychodzi atak")]
    [SerializeField] private Transform attackTransform;

    [Tooltip("Zasi�g ataku")]
    [SerializeField] private float attackRange = 0.5f;

    [Tooltip("Warstwy, kt�re mog� otrzyma� obra�enia")]
    [SerializeField] private LayerMask attackableLayer;

    [Header("Efekty")]
    [SerializeField] private AudioClip clawSound;
    [SerializeField] private string animationTrigger = "ClawAttack";


    private Animator anim;
    private AudioSource audioSource;

    public float CurrentCooldown { get; private set; }

    private void Update() {
        if (CurrentCooldown > 0) {
            CurrentCooldown -= Time.deltaTime;
        }
        else if (CurrentCooldown < 0) {
            CurrentCooldown = 0; 
        }
    }
    private void Awake() {
        anim = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();

        if (animationLockTime > attackCooldown) {
            Debug.LogWarning("Czas animacji jest d�u�szy ni� ca�kowity cooldown!");
        }
        CurrentCooldown = 0f;
    }

    public bool IsReady() {
        return isReady;
    }
    public void ExecuteAttack(AttackSelector selector) {
        StartCoroutine(AttackCoroutine(selector));
    }

    private IEnumerator AttackCoroutine(AttackSelector selector) {
        isReady = false;
        anim.SetTrigger(animationTrigger);
        if (clawSound != null && audioSource != null) {
            audioSource.PlayOneShot(clawSound);
        }

        PerformDamageCheck();

        yield return new WaitForSeconds(animationLockTime);

        selector.SetState(AttackSelector.PlayerState.Idle);

        yield return new WaitForSeconds(attackCooldown - animationLockTime);
        isReady = true;
        Debug.Log("Claw Attack jest gotowy!");
    }

    private void PerformDamageCheck() {
        if (attackTransform == null) return;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hits.Length; i++) {
            IDamageable damageable = hits[i].collider.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.Damage(damageAmount);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (attackTransform != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}