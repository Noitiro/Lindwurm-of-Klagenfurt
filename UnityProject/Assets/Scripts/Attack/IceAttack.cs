using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IceAttack : BaseAttack {
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
            Debug.Log("Lód zamroził: ");
        }
        
    }
}