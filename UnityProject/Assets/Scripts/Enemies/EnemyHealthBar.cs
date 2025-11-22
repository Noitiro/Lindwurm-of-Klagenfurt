using UnityEngine;
using UnityEngine.UI; 
public class EnemyHealthBar : MonoBehaviour {
    [SerializeField] private Image healthBarImage;

    [SerializeField] private EnemiesHealth enemyHealth;

    void Start() {

        if (enemyHealth == null) {
            enemyHealth = GetComponentInParent<EnemiesHealth>();
        }
    }
    void Update() {
        if (enemyHealth != null) {

            float fillValue = enemyHealth.CurrentHealth / enemyHealth.MaxHealth;

            healthBarImage.fillAmount = fillValue;
        }
        transform.rotation = Quaternion.identity;

        if (enemyHealth.CurrentHealth <= 0) {
            gameObject.SetActive(false);
        }
    }
}