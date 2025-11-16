using UnityEngine;

public class Arrows : MonoBehaviour {
    private int damage = 10;
    public float speed = 5f;
    private bool canAttack = false;


    private Vector2 targetPosition;
    private bool playerFound = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {

            IDamageable damageable = other.GetComponent<PlayerHealth>();
            damageable.Damage(damage);
        }
    }
    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            targetPosition = player.transform.position;
            playerFound = true;
            Debug.Log("ZNALEZIONO LOKALIZAJCE!!!!!");
        }
        else {
            Debug.LogError("Nie można znaleźć gracza!");
            Destroy(gameObject);
        }
    }

    void Update() {
        if (!playerFound) {
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f) {
            Destroy(gameObject);
            Debug.Log("DOTARŁ!!!!!!!");
        }
    }
}