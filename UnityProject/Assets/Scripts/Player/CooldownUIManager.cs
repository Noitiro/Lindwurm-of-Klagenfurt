using UnityEngine;
using TMPro; // Potrzebne do obs³ugi TextMeshPro

public class SimpleCooldownUI : MonoBehaviour {
    [Header("Referencja do Gracza")]
    [Tooltip("Przeci¹gnij tu obiekt Gracza, który ma 'AttackSelector'")]
    [SerializeField] private AttackSelector playerAttackSelector;

    [Header("Pola Tekstowe")]
    [SerializeField] private TextMeshProUGUI clawCooldownText;
    [SerializeField] private TextMeshProUGUI iceCooldownText;
    [SerializeField] private TextMeshProUGUI fireCooldownText;

    // Prywatne referencje do skryptów ataków
    private ClawAttack clawAttack;
    private IceAttack iceAttack;
    private FireBreathAttack fireBreathAttack;

    void Start() {
        // Sprawdzenie, czy podpi¹³eœ AttackSelector
        if (playerAttackSelector == null) {
            Debug.LogError("Nie podpiêto 'playerAttackSelector'!");
            this.enabled = false;
            return;
        }

        // Pobierz skrypty ataków z AttackSelectora
        // (Upewnij siê, ¿e w AttackSelector doda³eœ publiczne w³aœciwoœci)
        clawAttack = playerAttackSelector.ClawAttackScript;
        iceAttack = playerAttackSelector.IceAttackScript;
        fireBreathAttack = playerAttackSelector.FireBreathAttackScript;

        // Zresetuj teksty na starcie
        UpdateText(clawAttack, clawCooldownText);
        UpdateText(iceAttack, iceCooldownText);
        UpdateText(fireBreathAttack, fireCooldownText);
    }

    void Update() {
        // Aktualizuj wszystkie teksty w ka¿dej klatce
        UpdateText(clawAttack, clawCooldownText);
        UpdateText(iceAttack, iceCooldownText);
        UpdateText(fireBreathAttack, fireCooldownText);
    }

    /// <summary>
    /// G³ówna funkcja aktualizuj¹ca tekst na podstawie cooldownu
    /// </summary>
    private void UpdateText(MonoBehaviour attackScript, TextMeshProUGUI textElement) {
        // SprawdŸ, czy atak i tekst s¹ pod³¹czone
        if (attackScript == null || textElement == null) return;

        // Pobierz wartoœci z konkretnego skryptu ataku
        // (Musimy rzutowaæ typ, aby dostaæ siê do 'CurrentCooldown')
        float currentCooldown = 0f;

        if (attackScript is ClawAttack)
            currentCooldown = ((ClawAttack)attackScript).CurrentCooldown;
        else if (attackScript is IceAttack)
            currentCooldown = ((IceAttack)attackScript).CurrentCooldown;
        else if (attackScript is FireBreathAttack)
            currentCooldown = ((FireBreathAttack)attackScript).CurrentCooldown;


        // Zdecyduj, co pokazaæ
        if (currentCooldown > 0) {
            // --- Atak jest na cooldownie ---
            textElement.gameObject.SetActive(true); // Poka¿ tekst
            textElement.text = currentCooldown.ToString("F1"); // Poka¿ np. "2.1"
        }
        else {
            // --- Atak jest gotowy ---
            textElement.gameObject.SetActive(false); // Ukryj tekst
        }
    }
}