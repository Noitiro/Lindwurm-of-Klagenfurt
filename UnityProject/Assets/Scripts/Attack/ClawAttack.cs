using UnityEngine;
using System.Collections.Generic;

public class ClawAttack : BaseAttack {
    protected override void PerformAttackLogic() {
        if (attackHitbox == null) {
            Debug.LogWarning("Brak przypisanego Hitboxa (Collidera)!");
            return;
        }

        List<Collider2D> hits = new List<Collider2D>();
        attackHitbox.OverlapCollider(contactFilter, hits);

        List<IDamageable> targets = GetUniqueTargets(hits); 

        foreach (var target in targets) {
            target.Damage(damageAmount);
        }
    }
}