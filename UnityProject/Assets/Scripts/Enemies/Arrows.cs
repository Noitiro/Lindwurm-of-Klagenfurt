using UnityEngine;

public class Arrows : MonoBehaviour {
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 5f;

    private Vector2 targetPosition;
    private bool playerFound = false;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            targetPosition = player.transform.position;
            playerFound = true;
            RotateTowardsTarget();
        }
        else {
            Destroy(gameObject);
        }

        Destroy(gameObject, 10f);
    }

    void Update() {
        if (!playerFound) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f) {
            Destroy(gameObject);
        }
    }

    private void RotateTowardsTarget() {
        Vector2 direction = targetPosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            IDamageable damageable = other.GetComponent<IDamageable>(); 
            if (damageable != null) {
                damageable.Damage(damage);
            }
            Destroy(gameObject);
        }
    }
}