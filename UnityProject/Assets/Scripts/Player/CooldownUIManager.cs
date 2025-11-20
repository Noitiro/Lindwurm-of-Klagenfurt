using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SimpleCooldownUI : MonoBehaviour {
    private AttackSelector playerAttackSelector;

    [Header("Pola Tekstowe (Liczniki)")]
    [SerializeField] private TextMeshProUGUI clawCooldownText;
    [SerializeField] private TextMeshProUGUI iceCooldownText;
    [SerializeField] private TextMeshProUGUI fireCooldownText;
    [SerializeField] private TextMeshProUGUI tailCooldownText;

    [Header("Podœwietlenie Ataków (Obrazki)")]
    [SerializeField] private Image clawHighlightImage;
    [SerializeField] private Image iceHighlightImage;
    [SerializeField] private Image fireHighlightImage;
    [SerializeField] private Image tailHighlightImage;

    [Header("Ustawienia Podœwietlenia")]
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [SerializeField] private float selectedScale = 0.5f;
    [SerializeField] private float defaultScale = 0.5f;
    [SerializeField] private float transitionSpeed = 15f;

    private Image[] highlightImages;
    private Color[] targetColors;
    private Vector3[] targetScales;

    private ClawAttack clawAttack;
    private IceAttack iceAttack;
    private FireBreathAttack fireBreathAttack;
    private TailAttack tailAttack;

    void Start() {
        highlightImages = new Image[] { clawHighlightImage, iceHighlightImage, fireHighlightImage, tailHighlightImage };
        targetColors = new Color[4];
        targetScales = new Vector3[4];

        for (int i = 0; i < 4; i++) {
            targetColors[i] = defaultColor;
            targetScales[i] = Vector3.one * defaultScale;
            if (highlightImages[i] != null) {
                highlightImages[i].color = defaultColor;
                highlightImages[i].transform.localScale = Vector3.one * defaultScale;
            }
            FindPlayer();
        }
    }

    void Update() {
        if (playerAttackSelector == null) {
            FindPlayer();
            if (playerAttackSelector == null) return;
        }

        // Aktualizuj teksty
        UpdateText(clawAttack, clawCooldownText);
        UpdateText(iceAttack, iceCooldownText);
        UpdateText(fireBreathAttack, fireCooldownText);
        UpdateText(tailAttack, tailCooldownText);

        // Animacja ikonek
        for (int i = 0; i < highlightImages.Length; i++) {
            if (highlightImages[i] == null) continue;

            highlightImages[i].color = Color.Lerp(highlightImages[i].color, targetColors[i], Time.deltaTime * transitionSpeed);
            highlightImages[i].transform.localScale = Vector3.Lerp(highlightImages[i].transform.localScale, targetScales[i], Time.deltaTime * transitionSpeed);
        }
    }

    private void FindPlayer() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            playerAttackSelector = player.GetComponent<AttackSelector>();

            if (playerAttackSelector != null) {
                AttackSelector.OnAttackSelected -= HandleAttackSelectionChanged;
                AttackSelector.OnAttackSelected += HandleAttackSelectionChanged;

                clawAttack = playerAttackSelector.ClawAttackScript;
                iceAttack = playerAttackSelector.IceAttackScript;
                fireBreathAttack = playerAttackSelector.FireBreathAttackScript;
                tailAttack = playerAttackSelector.TailAttackScript;

                HandleAttackSelectionChanged(playerAttackSelector.CurrentAttackType);

            }
        }
    }

    private void OnDestroy() {
        AttackSelector.OnAttackSelected -= HandleAttackSelectionChanged;
    }

    private void HandleAttackSelectionChanged(AttackSelector.AttackType newAttack) {
        int selectedIndex = (int)newAttack;

        for (int i = 0; i < highlightImages.Length; i++) {
            if (highlightImages[i] == null) continue;

            if (i == selectedIndex) {
                targetColors[i] = highlightedColor;
                targetScales[i] = Vector3.one * selectedScale;
            }
            else {
                targetColors[i] = defaultColor;
                targetScales[i] = Vector3.one * defaultScale;
            }
        }
    }

    private void UpdateText(MonoBehaviour attackScript, TextMeshProUGUI textElement) {
        if (attackScript == null || textElement == null) return;

        float currentCooldown = 0f;

        if (attackScript is ClawAttack)
            currentCooldown = ((ClawAttack)attackScript).CurrentCooldown;
        else if (attackScript is IceAttack)
            currentCooldown = ((IceAttack)attackScript).CurrentCooldown;
        else if (attackScript is FireBreathAttack)
            currentCooldown = ((FireBreathAttack)attackScript).CurrentCooldown;
        else if (attackScript is TailAttack)
            currentCooldown = ((TailAttack)attackScript).CurrentCooldown;

        if (currentCooldown > 0) {
            textElement.gameObject.SetActive(true);
            textElement.text = currentCooldown.ToString("F1");
        }
        else {
            textElement.gameObject.SetActive(false);
        }
    }
}