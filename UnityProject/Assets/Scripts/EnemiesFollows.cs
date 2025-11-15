using UnityEngine;

public class EnemiesFollows : MonoBehaviour {

    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        GameObject player = GameObject.Find("Player");
        if (player != null) {
            target = player.transform;
        }
    }

    void Update() {
        if (target != null) {
            Vector2 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }

    void FixedUpdate() {
        if (target != null) {
            rb.linearVelocity = moveDirection * speed;
        }
    }
}
