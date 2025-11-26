using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MeleeEnemyAttack : MonoBehaviour {

    [Header("Statystyki")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private float windUpTime = 0.5f;

    [Tooltip(" prêdkoœc podczas zamachu. 0 = stoi 1 = biegnie")]
    [SerializeField] private float windUpMoveSpeedMultiplier = 0.4f; 

    [Header("Sygnalizacja")]
    [SerializeField] private GameObject alertIcon;
    [SerializeField] private string windUpAnimationTrigger;

    [Header("Efekty Uderzenia")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private string attackAnimationTrigger;

    private EnemiesFollowsAI movementScript;
    private NavMeshAgent agent;
    private Animator anim;
    private IDamageable targetPlayer;
    private Coroutine attackCoroutine;
    private float originalSpeed; 

    private void Awake() {
        movementScript = GetComponent<EnemiesFollowsAI>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

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

            if (agent != null) {
                agent.speed = originalSpeed * windUpMoveSpeedMultiplier;
            }

            if (alertIcon != null) alertIcon.SetActive(true);
            if (!string.IsNullOrEmpty(windUpAnimationTrigger) && anim != null) {
                anim.SetTrigger(windUpAnimationTrigger);
            }

            yield return new WaitForSeconds(windUpTime);

            if (agent != null) agent.speed = 0; 

            if (alertIcon != null) alertIcon.SetActive(false);
            if (!string.IsNullOrEmpty(attackAnimationTrigger) && anim != null) {
                anim.SetTrigger(attackAnimationTrigger);
            }


            if (targetPlayer != null && Vector3.Distance(transform.position, (targetPlayer as Component).transform.position) < 6f) {
                targetPlayer.Damage(damage);
            }

            yield return new WaitForSeconds(attackCooldown);

            if (agent != null) agent.speed = originalSpeed;
        }

        if (agent != null) agent.speed = originalSpeed;
        if (alertIcon != null) alertIcon.SetActive(false);
        attackCoroutine = null;
    }
}