using UnityEngine;
using System.Collections;

public class ArcherAttack : MonoBehaviour {
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Ustawienia Strzelania")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    // --- NOWA ZMIENNA ---
    // Referencja do skryptu zdrowia gracza
    private PlayerHealth playerHealth;

    void Start() {
        if (arrowPrefab == null || arrowSpawnPoint == null) {
            Debug.LogError(gameObject.name + ": Prefabrykat strza³y lub punkt spawnu nie s¹ ustawione!");
            this.enabled = false;
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) {
            Debug.LogWarning(gameObject.name + ": Nie znaleziono gracza w scenie. Wy³¹czam strzelanie.");
            this.enabled = false;
            return;
        }

        playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null) {
            StartCoroutine(ShootLoop());
        }
        else {
            Debug.LogError(gameObject.name + ": Nie mo¿na znaleŸæ komponentu PlayerHealth na graczu!");
            this.enabled = false;
        }
    }

    IEnumerator ShootLoop() {
        while (playerHealth != null && playerHealth.currentHealth > 0) {
            Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            Debug.Log(gameObject.name + " wystrzeli³ strza³ê!");

            yield return new WaitForSeconds(attackCooldown);
        }

        Debug.Log(gameObject.name + ": Gracz nie ¿yje. Przestajê strzelaæ.");
    }
}