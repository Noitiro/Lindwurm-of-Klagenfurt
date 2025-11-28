using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private UpgradeCardSO currentCard;
    private UpgradeManager manager;
    private void Awake() {
        // Automatycznie podepnij klikniêcie
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    public void Setup(UpgradeCardSO card, UpgradeManager mgr) {
        currentCard = card;
        manager = mgr;

        nameText.text = card.cardName;
        descText.text = card.description;
        costText.text = card.cost + " Gold";
        iconImage.sprite = card.icon;

        // --- DIAGNOSTYKA ---
        int currentGold = ScoreManager.Instance.CurrentScore;
        Debug.Log($"Karta: {card.cardName}, Koszt: {card.cost}, Masz: {currentGold}");

        // SprawdŸ czy gracza staæ
        bool canAfford = currentGold >= card.cost;
        button.interactable = canAfford;
        costText.color = canAfford ? Color.yellow : Color.red;
    }

    public void OnClick() {
        if (manager != null) manager.SelectUpgrade(currentCard);
    }

}