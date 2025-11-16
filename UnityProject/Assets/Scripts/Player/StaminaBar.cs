using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Image stamina;
    public void Update() {
           stamina.fillAmount = playerMovement.currentEnergy / 100f;
    }
}
