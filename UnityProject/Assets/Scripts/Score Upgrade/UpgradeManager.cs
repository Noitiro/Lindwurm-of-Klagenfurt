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

    [Header("Referencja do Gracza")]
    [SerializeField] private AttackSelector playerAttacks;
    [SerializeField] private PlayerHealth playerHealth;

    private WaveSpawn waveSpawner; 

    private void Awake() { Instance = this; }

    private void Start() {
        upgradePanel.SetActive(false);
        waveSpawner = FindObjectOfType<WaveSpawn>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerAttacks = player.GetComponent<AttackSelector>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }
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

        List<UpgradeCardSO> deck = new List<UpgradeCardSO>(allUpgrades);

        for (int i = 0; i < 3; i++) {
            if (deck.Count == 0) break;

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
                // if (card.type == UpgradeCardSO.UpgradeType.MoveSpeed) playerMovement.UpgradeSpeed(card.value);

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