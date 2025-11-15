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
            // ZnaleŸliœmy gracza, przypisujemy go jako cel
            target = playerObject.transform;
        }
        else {
            // Nie znaleŸliœmy gracza - logujemy b³¹d i wy³¹czamy ten skrypt
            Debug.LogError("Nie znaleziono obiektu z tagiem 'Player' dla wroga: " + gameObject.name);
            this.enabled = false; // Wy³¹cza ten komponent (Update() nie bêdzie siê wykonywaæ)
        }
    }

    private void Update() {
        if (target != null) {
            agent.SetDestination(target.position);
        }
    }
}
