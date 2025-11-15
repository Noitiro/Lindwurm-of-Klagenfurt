using UnityEngine;
using UnityEngine.AI;

public class EnemiesFollowsAI : MonoBehaviour {

    private Transform target;

    NavMeshAgent agent;
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) {
            target = playerObject.transform;
        }
        else {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'Player' dla wroga: " + gameObject.name);
            this.enabled = false;
        }
    }

    private void Update() {
        if (target != null) {
            agent.SetDestination(target.position);
        }
    }
}
