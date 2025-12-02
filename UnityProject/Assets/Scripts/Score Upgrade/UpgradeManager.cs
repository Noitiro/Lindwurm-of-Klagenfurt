using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour {
    public static UpgradeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Transform cardsContainer; 
    [SerializeField] private GameObject cardPrefab; 

    [Header("Baza Kart")]
    [SerializeField] private List<UpgradeCardSO> allUpgrades;

    private List<UpgradeCardSO> availableCards;
    private Dictionary<UpgradeCardSO, int> purchaseHistory = new Dictionary<UpgradeCardSO, int>();

    [Header("Referencja do Gracza")]
    [SerializeField] private AttackSelector playerAttacks;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement; 

    private WaveSpawn waveSpawner; 

    private void Awake() { Instance = this; }

    private void Start() {
        upgradePanel.SetActive(false);
        waveSpawner = FindObjectOfType<WaveSpawn>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerAttacks = player.GetComponent<AttackSelector>();
            playerHealth = player.GetComponent<PlayerHealth>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        availableCards = new List<UpgradeCardSO>(allUpgrades);
        purchaseHistory.Clear();
    }

    public void OpenUpgradeMenu() {
        if (allUpgrades.Count == 0) {
            CloseMenu(); 
            return;
        }

        upgradePanel.SetActive(true);
        GenerateCards();

         Time.timeScale = 0; 
    }

    private void GenerateCards() {
        foreach (Transform child in cardsContainer) Destroy(child.gameObject);

        List<UpgradeCardSO> deck = new List<UpgradeCardSO>(availableCards);
        int cardsToDraw = Mathf.Min(3, deck.Count);

        for (int i = 0; i < cardsToDraw; i++) {
            int randomIndex = Random.Range(0, deck.Count);
            UpgradeCardSO cardData = deck[randomIndex];
            deck.RemoveAt(randomIndex);

            GameObject cardObj = Instantiate(cardPrefab, cardsContainer);
            cardObj.GetComponent<UpgradeCardUI>().Setup(cardData, this);
        }
    }

    public void SelectUpgrade(UpgradeCardSO card) {
        if (!ScoreManager.Instance.SpendScore(card.cost)) return;

        ApplyStats(card);
        if (!purchaseHistory.ContainsKey(card)) {
            purchaseHistory[card] = 0;
        }
        purchaseHistory[card]++;

        Debug.Log($"Kupiono {card.cardName}. Razem: {purchaseHistory[card]} / {card.maxPurchases}");

        if (card.maxPurchases != -1) {
            if (purchaseHistory[card] >= card.maxPurchases) {
                if (availableCards.Contains(card)) {
                    availableCards.Remove(card);
                    Debug.Log("Limit karty osi¹gniêty! Usuniêto ze sklepu.");
                }
            }
        }
        CloseMenu();
    }

    public void SkipUpgrade() // przycisk "Skip / Next Wave"
    {
        Debug.Log("Gracz pomin¹³ sklep.");
        CloseMenu();
    }

    private void CloseMenu() {
        Time.timeScale = 1f;
        upgradePanel.SetActive(false);
   
        if (waveSpawner != null) waveSpawner.NextWave();
    }

    private void ApplyStats(UpgradeCardSO card) {
        BaseAttack targetAttack = null;

        switch (card.target) {
            case UpgradeCardSO.UpgradeTarget.Claw: targetAttack = playerAttacks.ClawAttackScript; break;
            case UpgradeCardSO.UpgradeTarget.Ice: targetAttack = playerAttacks.IceAttackScript; break;
            case UpgradeCardSO.UpgradeTarget.Fire: targetAttack = playerAttacks.FireBreathAttackScript; break;
            case UpgradeCardSO.UpgradeTarget.Tail: targetAttack = playerAttacks.TailAttackScript; break;

            case UpgradeCardSO.UpgradeTarget.Player:
                if (card.type == UpgradeCardSO.UpgradeType.MaxHealth) playerHealth.UpgradeMaxHealth(card.value);
                if (card.type == UpgradeCardSO.UpgradeType.Regen) playerHealth.UpgradeRegen(card.value);

                // Speed w Player:
                 if (card.type == UpgradeCardSO.UpgradeType.MoveSpeed) playerMovement.UpgradeSpeed(card.value);

                return;
        }
        if (targetAttack != null) {
            switch (card.type) {
                case UpgradeCardSO.UpgradeType.Damage:
                    targetAttack.UpgradeDamage(card.value);
                    break;

                case UpgradeCardSO.UpgradeType.AreaSize:
                    targetAttack.UpgradeArea(card.value);
                    break;

                case UpgradeCardSO.UpgradeType.Knockback:
                    targetAttack.UpgradeKnockback(card.value); 
                    break;

                case UpgradeCardSO.UpgradeType.CritChance:
                    targetAttack.UpgradeCrit(card.value); 
                    break;
            }
        }
    }
}