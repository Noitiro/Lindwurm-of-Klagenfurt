using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemiesFollowsAI : MonoBehaviour {

    private Transform target;
    private float defaultSpeed;
    private NavMeshAgent agent;
    private Coroutine slowCoroutine;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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
        if (target != null && agent.enabled && agent.isOnNavMesh) {
            agent.SetDestination(target.position);
        }
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