using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private int damage = 1;
    public void AttackEnemy(GameObject enemy) {
        EnemiesHealth health = enemy.GetComponent<EnemiesHealth>();
        if (health != null) {
            health.TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            AttackEnemy(collision.gameObject);
        }
    }
}
