using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image currentHealthBar; 

    private void Start() {
        if (playerHealth == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerHealth = player.GetComponent<PlayerHealth>();
        }

        if (playerHealth != null) {
            playerHealth.OnHealthChanged += UpdateBar;

            UpdateBar(playerHealth.CurrentHealth / playerHealth.MaxHealth);
        }
    }

    private void OnDestroy() {

        if (playerHealth != null) {
            playerHealth.OnHealthChanged -= UpdateBar;
        }
    }

    private void UpdateBar(float pct) {
        currentHealthBar.fillAmount = pct;
    }
}