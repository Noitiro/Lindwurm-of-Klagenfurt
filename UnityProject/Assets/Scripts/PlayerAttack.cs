using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private int damage = 1;
    public void AttackEnemy(GameObject enemy) {
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null) {
            damageable.Damage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            AttackEnemy(collision.gameObject);
        }
    }
}
