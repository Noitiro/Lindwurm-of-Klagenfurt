using System.Collections.Generic;
using UnityEngine;

public class IceAttack : BaseAttack {
    protected override void PerformAttackLogic() {
        if (attackHitbox == null) {
            Debug.LogWarning("Brak przypisanego Hitboxa (Collidera)!");
            return;
        }

        List<Collider2D> hits = new List<Collider2D>();
        // Pamiętaj: Używamy OverlapCollider, nie Overlap!
        attackHitbox.Overlap(contactFilter, hits);

        List<IDamageable> targets = GetUniqueTargets(hits);

        foreach (var target in targets) {
            BaseEnemyHealth enemyScript = target as BaseEnemyHealth;
            float calculatedDmg = CalculateDamage(enemyScript);

            target.Damage(calculatedDmg);

            // --- DODANY EFEKT ---
            if (target is Component targetComponent) {
                ApplyHitFeedback(targetComponent.transform.position);
            }
        }
    }
}