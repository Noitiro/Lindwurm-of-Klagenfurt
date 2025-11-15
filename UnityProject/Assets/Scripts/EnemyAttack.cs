using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1.5f;
    private bool canAttack = true;
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && canAttack) {

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            
            if (health != null) {
                health.TakeDamage(damage);

                StartCoroutine(AttackCooldown());
            }
        }
    }
    IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}