using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemiesFollowsAI : MonoBehaviour {

    private Transform target;
    private float defaultSpeed;
    private NavMeshAgent agent;
    private Coroutine slowCoroutine;
    private Vector3 initialScale;
    private Animator anim;

    [Header("Tryb Odwracania")]
    [Tooltip("Zmienia animacjê boolem 'IsFacingRight'. Jesli ODZNACZONE: Obraca obiekt fizycznie (Scale X).")]
    [SerializeField] private bool useAnimationFlip = false;

    [Tooltip("Tylko dla fizycznego odwracania: czy grafika patrzy w lewo")]
    [SerializeField] private bool spriteFacesLeft = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        initialScale = transform.localScale;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            target = playerObject.transform;
        }
        else {
            this.enabled = false;
        }

        if (agent != null) defaultSpeed = agent.speed;
    }

    private void Update() {
        if (target != null) {
            if (agent.enabled && agent.isOnNavMesh) {
                agent.SetDestination(target.position);
            }

            float directionX = target.position.x - transform.position.x;

            if (useAnimationFlip) {
                if (anim != null) {
                    bool isRight = directionX > 0;
                    anim.SetBool("IsFacingRight", isRight);
                }
            }
            else {
                FlipEnemy(directionX);
            }
        }
    }

    private void FlipEnemy(float directionX) {
        float direction = directionX > 0 ? 1f : -1f;
        if (spriteFacesLeft) direction *= -1f;
        transform.localScale = new Vector3(Mathf.Abs(initialScale.x) * direction, initialScale.y, initialScale.z);
    }
    public void ApplySlow(float slowFactor, float duration) {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowCoroutine(slowFactor, duration));
    }

    public void RemoveSlow() {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        if (agent != null) agent.speed = defaultSpeed;
        GetComponent<SpriteRenderer>().color = Color.white;
        slowCoroutine = null;
    }

    private IEnumerator SlowCoroutine(float slowFactor, float duration) {
        if (agent == null) yield break;
        agent.speed = defaultSpeed * slowFactor;
        GetComponent<SpriteRenderer>().color = Color.cyan;
        yield return new WaitForSeconds(duration);
        agent.speed = defaultSpeed;
        GetComponent<SpriteRenderer>().color = Color.white;
        slowCoroutine = null;
    }
}