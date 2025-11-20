using UnityEngine;

public class ClawAttack : BaseAttack {
    protected override void PerformAttackLogic() {
        if (attackTransform == null) return;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        foreach (var hit in hits) {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.Damage(damageAmount);
            }
        }
    }
    protected override void OnDrawGizmosSelected() {
        if (attackTransform != null) {
            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}