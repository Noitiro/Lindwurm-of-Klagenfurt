using UnityEngine;

public class Arrows : MonoBehaviour {
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 5f;
    // 'canAttack' nie było używane, więc usunąłem

    private Vector2 targetPosition; // Pozycja, do której leci (ustalona w Start)
    private bool playerFound = false;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            targetPosition = player.transform.position; // Zapisz pozycję gracza TYLKO RAZ
            playerFound = true;
            Debug.Log("ZNALEZIONO LOKALIZAJCE!!!!!");
        }
        else {
            Debug.LogError("Nie można znaleźć gracza!");
            Destroy(gameObject);
        }

        // Dodaj zabezpieczenie, aby strzała zniknęła, jeśli w nic nie trafi
        Destroy(gameObject, 10f); // Zniszcz strzałę po 10 sekundach
    }

    void Update() {
        if (!playerFound) {
            return;
        }

        // Przesuń strzałę w kierunku ZAPISANEJ pozycji
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Ta logika jest DOBRA - niszczy strzałę, jeśli CHYBI i dotrze do celu
        if (Vector2.Distance(transform.position, targetPosition) < 0.01f) {
            Destroy(gameObject);
            Debug.Log("DOTARŁ (CHYBIŁ)!!!!!!!");
        }
    }

    // --- KLUCZOWA POPRAWKA JEST TUTAJ ---
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Poprawiony sposób pobierania IDamageable
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.Damage(damage);
            }

            // ZNISZCZ STRZAŁĘ NATYCHMIAST PO TRAFIENIU
            Destroy(gameObject);
            Debug.Log("TRAFIONO GRACZA!");
        }
    }
}