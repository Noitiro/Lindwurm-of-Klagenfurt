using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    PlayerManager playerManager;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start() {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    private void Update() {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
