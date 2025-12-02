using System.Collections.Generic;
using UnityEngine;

public class IceAttack : BaseAttack {
    [Header("Specjalne Efekty Lodu")]
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float slowDuration = 2f;

    protected override void PerformAttackLogic() {
        if (attackHitbox == null) {
            Debug.LogWarning("Brak przypisanego Hitboxa (Collidera)!");
            return;
        }

        List<Collider2D> hits = new List<Collider2D>();
        attackHitbox.Overlap(contactFilter, hits); 

        List<IDamageable> targets = GetUniqueTargets(hits);

        foreach (var target in targets) {
            BaseEnemyHealth enemyScript = target as BaseEnemyHealth;
            float calculatedDmg = CalculateDamage(enemyScript);

            target.Damage(calculatedDmg);
            if (target is BaseEnemyHealth enemy) {
                enemy.Flash(Color.cyan);
                if (enemyScript != null) {
                    enemyScript.ApplyFreeze(slowFactor, slowDuration);
                }
            }
            if (target is Component targetComponent) {
                ApplyHitFeedback(targetComponent.gameObject);
            }
        }
    }
}