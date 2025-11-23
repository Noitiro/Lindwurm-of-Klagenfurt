using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyKnockback : MonoBehaviour {

    private Rigidbody2D rb;
    private NavMeshAgent agent;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void ApplyKnockback(Transform damageSource, float force) {
        // Oblicz kierunek odpychania
        Vector2 direction = (transform.position - damageSource.position).normalized;
        StartCoroutine(KnockbackCoroutine(direction, force));
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Zablokuj rotację na sztywno
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force) {
        // 1. Wy��cz AI - to naprawi b��d "SetDestination"
        if (agent != null) {
            agent.enabled = false;
        }

        // 2. Zmie� cia�o na Dynamiczne, �eby reagowa�o na si��
        rb.bodyType = RigidbodyType2D.Dynamic;

        // 3. Pchnij wroga
        rb.linearVelocity = Vector2.zero; // Reset pr�dko�ci
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // 4. Czekaj na koniec odrzutu
        yield return new WaitForSeconds(0.2f);

        // 5. Zatrzymaj wroga ca�kowicie
        rb.linearVelocity = Vector2.zero;

        // 6. Przywr�� stan Kinematic
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 7. W��cz AI z powrotem
        // (Czekamy jedn� klatk�, �eby pozycja fizyczna si� zsynchronizowa�a)
        yield return null;

        if (agent != null) {
            // Upewnij si�, �e agent jest na NavMesh'u przed w��czeniem
            agent.Warp(transform.position);
            agent.enabled = true;
        }
    }
}