using UnityEngine;
using System.Collections;

public class KnightAttack : MonoBehaviour {
    private int damage = 3;
    private float attackCooldown = 3f;
    private bool canAttack = true;
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && canAttack) {

            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.Damage(damage);
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