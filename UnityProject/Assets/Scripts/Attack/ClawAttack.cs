using UnityEngine;
using System.Collections.Generic;

public class ClawAttack : BaseAttack {
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
                enemy.Flash(Color.grey);
            }
            if (target is Component targetComponent) {
                ApplyHitFeedback(targetComponent.gameObject);
            }
        }
    }
}