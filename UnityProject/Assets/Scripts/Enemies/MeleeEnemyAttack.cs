using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class MeleeEnemyAttack : MonoBehaviour {

    [Header("Statystyki")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private float windUpTime = 0.5f;

    [Tooltip("Prêdkoœæ podczas zamachu. 0 = stoi, 1 = biegnie normalnie")]
    [SerializeField] private float windUpMoveSpeedMultiplier = 0.4f;

    [Header("Sygnalizacja")]
    [SerializeField] private GameObject alertIcon;
    [SerializeField] private string windUpAnimationTrigger;

    [Header("Efekty Uderzenia (Audio)")]
    [SerializeField] private AudioClip[] attackSounds; 

    [Tooltip("0.8 = gruby g³os, 1.2 = piskliwy. Ustaw np. 0.9 - 1.1")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    [SerializeField] private string attackAnimationTrigger;

    private EnemiesFollowsAI movementScript;
    private NavMeshAgent agent;
    private Animator anim;
    private IDamageable targetPlayer;
    private Coroutine attackCoroutine;
    private float originalSpeed;
    private AudioSource audioSource;

    private void Awake() {
        movementScript = GetComponent<EnemiesFollowsAI>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        if (alertIcon != null) alertIcon.SetActive(false);
    }

    private void Start() {
        if (agent != null) originalSpeed = agent.speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            targetPlayer = other.GetComponent<IDamageable>();
            if (targetPlayer != null && attackCoroutine == null) {
                attackCoroutine = StartCoroutine(AttackRoutine());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            targetPlayer = null;
        }
    }

    private IEnumerator AttackRoutine() {
        while (targetPlayer != null) {

            // 1. FAZA ZAMACHU (WIND-UP)
            if (agent != null) agent.speed = originalSpeed * windUpMoveSpeedMultiplier;
            if (alertIcon != null) alertIcon.SetActive(true);
            if (!string.IsNullOrEmpty(windUpAnimationTrigger) && anim != null) anim.SetTrigger(windUpAnimationTrigger);

            // Czekamy... (Gracz mo¿e tu uciec!)
            yield return new WaitForSeconds(windUpTime);

            // --- NOWOŒÆ: SPRAWDZENIE CZY GRACZ NADAL JEST ---
            // Jeœli gracz wyszed³ z Triggera w trakcie zamachu -> PRZERWIJ ATAK
            if (targetPlayer == null) {
                // Posprz¹taj po sobie i wyjdŸ
                if (agent != null) agent.speed = originalSpeed;
                if (alertIcon != null) alertIcon.SetActive(false);
                attackCoroutine = null;
                yield break; // Zakoñcz korutynê natychmiast
            }
            // -----------------------------------------------

            // 2. FAZA UDERZENIA (STRIKE)
            if (agent != null) agent.speed = 0;
            if (alertIcon != null) alertIcon.SetActive(false);
            if (!string.IsNullOrEmpty(attackAnimationTrigger) && anim != null) anim.SetTrigger(attackAnimationTrigger);

            // DŸwiêk zagra tylko, jeœli gracz nadal tu jest (dziêki sprawdzeniu wy¿ej)
            if (audioSource != null && attackSounds.Length > 0) {
                AudioClip clipToPlay = attackSounds[Random.Range(0, attackSounds.Length)];
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(clipToPlay);
            }

            // Zadaj obra¿enia (sprawdzamy te¿ dystans fizyczny dla pewnoœci)
            if (targetPlayer != null && Vector3.Distance(transform.position, (targetPlayer as Component).transform.position) < 6f) {
                targetPlayer.Damage(damage);
            }

            // 3. FAZA ODPOCZYNKU
            yield return new WaitForSeconds(attackCooldown);

            if (agent != null) agent.speed = originalSpeed;
        }

        // Sprz¹tanie po wyjœciu z pêtli
        if (agent != null) agent.speed = originalSpeed;
        if (alertIcon != null) alertIcon.SetActive(false);
        attackCoroutine = null;
    }
}