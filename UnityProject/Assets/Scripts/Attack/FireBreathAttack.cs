using System.Collections.Generic;
using UnityEngine;

public class FireBreathAttack : BaseAttack {

    [Header("Specjalne Efekty Ognia")]
    [SerializeField] private float burnDamage = 5f; 
    [SerializeField] private int burnTicks = 3;     
    [SerializeField] private float burnInterval = 1f;

    protected override void PerformAttackLogic() {
        if (attackHitbox == null) return;

        List<Collider2D> hits = new List<Collider2D>();
        attackHitbox.Overlap(contactFilter, hits);
        List<IDamageable> targets = GetUniqueTargets(hits);

        foreach (var target in targets) {

            if (target is DestroyHouses house) {
                house.Burn(); 

                if (target is Component houseComp) ApplyHitFeedback(houseComp.gameObject);
                continue;
            }

            BaseEnemyHealth enemyScript = target as BaseEnemyHealth;
            float calculatedDmg = CalculateDamage(enemyScript);

            target.Damage(calculatedDmg);

            if (enemyScript != null) {

            }

            if (target is Component targetComponent) {
                ApplyHitFeedback(targetComponent.gameObject);
            }
        }
    }
}