using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {
    [SerializeField] private Image healthBarImage;

    [SerializeField] private BaseEnemyHealth enemyHealth;

    void Start() {

        if (enemyHealth == null) {
            enemyHealth = GetComponentInParent<BaseEnemyHealth>();
        }
    }

    void Update() {
        if (enemyHealth != null) {
            float fillValue = enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
            healthBarImage.fillAmount = fillValue;

            if (enemyHealth.CurrentHealth <= 0) {
                gameObject.SetActive(false);
            }
        }
        else {
            Destroy(gameObject);
        }
        transform.rotation = Quaternion.identity;
    }
}