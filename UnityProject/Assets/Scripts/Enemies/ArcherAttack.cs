using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class ArcherAttack : MonoBehaviour {

    [Header("Statystyki")]
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private float aimingTime = 1.0f;

    [Header("Zasiêgi")]
    [Tooltip("Zasiêg, w którym ³ucznik zaczyna walkê")]
    [SerializeField] private float detectRange = 15f;

    [Tooltip("Idealny dystans do strza³u")]
    [SerializeField] private float preferredDistance = 7f;

    [Tooltip("Jeœli gracz podejdzie bli¿ej ni¿ to, ³ucznik zacznie uciekaæ")]
    [SerializeField] private float fleeDistance = 4f; 

    [Header("Ustawienia Strzelania")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private LineRenderer laserSight;
    private PlayerHealth playerHealth;
    private Transform playerTransform;
    private NavMeshAgent agent;
    private EnemiesFollowsAI movementScript;

    void Start() {
        laserSight = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        movementScript = GetComponent<EnemiesFollowsAI>();

        laserSight.startWidth = 0.05f;
        laserSight.endWidth = 0.05f;
        laserSight.enabled = false;

        if (agent != null) agent.stoppingDistance = 0;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerTransform = player.transform;
            StartCoroutine(CombatLoop());
        }
        else {
            this.enabled = false;
        }
    }

    IEnumerator CombatLoop() {
        while (playerHealth != null && playerHealth.CurrentHealth > 0) {

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer < fleeDistance) {
               
                if (movementScript != null) movementScript.enabled = false;

                if (agent != null) {
                    agent.isStopped = false;
                    
                    Vector3 fleeDirection = (transform.position - playerTransform.position).normalized;
                    Vector3 fleeTarget = transform.position + fleeDirection * 5f;

                    agent.SetDestination(fleeTarget);
                }

                yield return new WaitForSeconds(0.2f); 
            }
            else if (distanceToPlayer > preferredDistance && distanceToPlayer < detectRange) {
                if (movementScript != null) movementScript.enabled = true;
                if (agent != null) agent.isStopped = false;

                yield return null;
            }
            else if (distanceToPlayer <= preferredDistance) {
                if (movementScript != null) movementScript.enabled = false;
                if (agent != null) {
                    agent.isStopped = true;
                    agent.ResetPath();
                    agent.velocity = Vector3.zero;
                }

                laserSight.enabled = true;
                float timer = 0f;
                while (timer < aimingTime) {
                    if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < fleeDistance) {
                        laserSight.enabled = false;
                        break; 
                    }

                    if (playerTransform != null) {
                        laserSight.SetPosition(0, arrowSpawnPoint.position);
                        laserSight.SetPosition(1, playerTransform.position);
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }


                if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < fleeDistance) {
                    continue; 
                }

                // C. STRZA£
                laserSight.enabled = false;
                if (arrowPrefab != null && arrowSpawnPoint != null) {
                    Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
                }


                float cooldownTimer = attackCooldown;
                while (cooldownTimer > 0) {
                    if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < fleeDistance) {
                        break; 
                    }
                    cooldownTimer -= Time.deltaTime;
                    yield return null;
                }

                if (agent != null) agent.isStopped = false;
            }
            else {
              
                if (movementScript != null) movementScript.enabled = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}