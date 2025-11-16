using UnityEngine;
using System.Collections;

public class ClawAttack : MonoBehaviour {
    [Header("Ustawienia Ataku")]
    [Tooltip("Ca³kowity czas odnowienia (np. 3 sekundy)")]
    [SerializeField] private float attackCooldown = 3f;

    [Tooltip("Czas trwania animacji ataku (np. 0.5 sekundy)")]
    [SerializeField] private float animationLockTime = 0.5f;

    [Tooltip("Iloœæ obra¿eñ")]
    [SerializeField] private float damageAmount = 100f;

    [Tooltip("Punkt, z którego wychodzi atak")]
    [SerializeField] private Transform attackTransform;

    [Tooltip("Zasiêg ataku")]
    [SerializeField] private float attackRange = 0.5f;

    [Tooltip("Warstwy, które mog¹ otrzymaæ obra¿enia")]
    [SerializeField] private LayerMask attackableLayer;

    [Header("Efekty")]
    [SerializeField] private AudioClip clawSound;
    [SerializeField] private string animationTrigger = "ClawAttack";

    // Prywatne komponenty
    private Animator anim;
    private AudioSource audioSource;

    // --- LOKALNY COOLDOWN ---
    // Ta zmienna œledzi TYLKO ten jeden atak
    private bool isReady = true;

    private void Awake() {
        // Pobierz komponenty z g³ównego obiektu gracza
        anim = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();

        // Zabezpieczenie na wypadek z³ego ustawienia
        if (animationLockTime > attackCooldown) {
            Debug.LogWarning("Czas animacji jest d³u¿szy ni¿ ca³kowity cooldown!");
        }
    }

    /// <summary>
    /// Zwraca 'true', jeœli ten atak nie jest na cooldownie.
    /// Wywo³ywane przez AttackSelector.
    /// </summary>
    public bool IsReady() {
        return isReady;
    }

    /// <summary>
    /// Uruchamia wykonanie ataku.
    /// Wywo³ywane przez AttackSelector.
    /// </summary>
    public void ExecuteAttack(AttackSelector selector) {
        // Rozpocznij korutynê, która zajmie siê logik¹ ataku i cooldownami
        StartCoroutine(AttackCoroutine(selector));
    }

    private IEnumerator AttackCoroutine(AttackSelector selector) {
        // 1. URUCHOM LOKALNY COOLDOWN (np. na 3 sekundy)
        // Ten atak staje siê niedostêpny
        isReady = false;

        // 2. WYKONAJ ATAK (animacja, dŸwiêk, obra¿enia)
        anim.SetTrigger(animationTrigger);
        if (clawSound != null && audioSource != null) {
            audioSource.PlayOneShot(clawSound);
        }

        // Tutaj dodaj logikê obra¿eñ (np. CircleCast)
        PerformDamageCheck();

        // 3. CZEKAJ NA KONIEC ANIMACJI (np. 0.5 sekundy)
        // To jest GLOBALNA blokada
        yield return new WaitForSeconds(animationLockTime);

        // 4. ZWOLNIJ GLOBALN¥ BLOKADÊ
        // Gracz mo¿e teraz biegaæ, wybieraæ i u¿ywaæ INNYCH ataków
        selector.SetState(AttackSelector.PlayerState.Idle);

        // 5. CZEKAJ RESZTÊ LOKALNEGO COOLDOWNU
        // (np. 3s - 0.5s = pozosta³e 2.5 sekundy)
        yield return new WaitForSeconds(attackCooldown - animationLockTime);

        // 6. ZWOLNIJ LOKALN¥ BLOKADÊ
        // Ten konkretny atak (Claw) jest znowu gotowy
        isReady = true;
        Debug.Log("Claw Attack jest gotowy!");
    }

    /// <summary>
    /// Sprawdza trafienia i zadaje obra¿enia
    /// </summary>
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

    /// <summary>
    /// Rysuje Gizmo w edytorze
    /// </summary>
    private void OnDrawGizmosSelected() {
        if (attackTransform != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}