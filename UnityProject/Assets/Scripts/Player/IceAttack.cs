using UnityEngine;
using System.Collections;

public class IceAttack : MonoBehaviour { 
    [Header("Ustawienia Ataku Lodowego")] 

    [Tooltip("Całkowity czas odnowienia (np. 3 sekundy)")]
    [SerializeField] private float attackCooldown = 3f;

    [Tooltip("Czas trwania animacji ataku (np. 0.5 sekundy)")]
    [SerializeField] private float animationLockTime = 0.5f;

    [Tooltip("Ilość obrażeń")]
    [SerializeField] private float damageAmount = 100f;

    [Tooltip("Punkt, z którego wychodzi atak")]
    [SerializeField] private Transform attackTransform;

    [Tooltip("Zasięg ataku")]
    [SerializeField] private float attackRange = 0.5f;

    [Tooltip("Warstwy, które mogą otrzymać obrażenia")]
    [SerializeField] private LayerMask attackableLayer;

    [Header("Efekty")]
    [SerializeField] private AudioClip iceSound;
    [SerializeField] private string animationTrigger = "IceAttack"; 

    private Animator anim;
    private AudioSource audioSource;
    private bool isReady = true;

    private void Awake() {
        anim = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();

        if (animationLockTime > attackCooldown) {
            Debug.LogWarning("Czas animacji jest dłuższy niż całkowity cooldown!");
        }
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

        if (iceSound != null && audioSource != null) {
            audioSource.PlayOneShot(iceSound);
        }

        PerformDamageCheck();
        yield return new WaitForSeconds(animationLockTime);
        selector.SetState(AttackSelector.PlayerState.Idle);
        yield return new WaitForSeconds(attackCooldown - animationLockTime);
        isReady = true;

        Debug.Log("Ice Attack jest gotowy!");
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
            Gizmos.color = Color.blue; 
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}