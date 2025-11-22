using System.Collections.Generic;
using UnityEngine;

public class FireBreathAttack : BaseAttack {
    protected override void PerformAttackLogic() {
        if (attackHitbox == null) {
            Debug.LogWarning("Brak przypisanego Hitboxa (Collidera)!");
            return;
        }

        List<Collider2D> hits = new List<Collider2D>();
        attackHitbox.Overlap(contactFilter, hits);

        List<IDamageable> targets = GetUniqueTargets(hits);

        foreach (var target in targets) {
            target.Damage(damageAmount);
            Debug.Log("Ogień podpalił cel!");
        }
    }
}