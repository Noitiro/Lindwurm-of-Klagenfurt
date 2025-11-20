using UnityEngine;
using System.Collections.Generic;

public class FireBreathAttack : BaseAttack {
    protected override void PerformAttackLogic() {
        if (attackHitbox == null) return;

        List<Collider2D> hits = new List<Collider2D>();
        attackHitbox.Overlap(contactFilter, hits);

        foreach (var hit in hits) {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.Damage(damageAmount);
                Debug.Log("Ogień podpalił: " + hit.name);
            }
        }
    }
}