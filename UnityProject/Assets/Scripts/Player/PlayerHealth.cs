using System.Collections;
using UnityEngine;
using System; 

public class PlayerHealth : MonoBehaviour, IDamageable {
    [Header("Statystyki")]
    [SerializeField] private float maxHealth = 200;
    public float CurrentHealth { get; private set; }

  
    public float MaxHealth => maxHealth;

    [Header("Regeneracja")]
    [SerializeField] private float regenAmount = 1f;
    [SerializeField] private float regenRate = 1f;

    [Header("UI")]
    [SerializeField] private Canvas gameOverScreen; 

    public event Action<float> OnHealthChanged;

    private bool isDead = false;
    private Animator anim;
    private Coroutine regenCoroutine;

    private void Awake() {
        anim = GetComponent<Animator>();
        CurrentHealth = maxHealth;
    }

    private void Start() {
        OnHealthChanged?.Invoke(1f);
        gameOverScreen = GetComponentInChildren<Canvas>();
    }

    public void Damage(float damage) {
        if (isDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);

        float healthPercent = CurrentHealth / maxHealth;
        OnHealthChanged?.Invoke(healthPercent);

        Debug.Log("Player damage: " + damage);

        if (CurrentHealth > 1) {
            anim.SetTrigger("hurt");
            StartRegen();
        }
        else {
            KillPlayer();
        }
    }

    public void Heal(float amount) {
        if (isDead) return;

        CurrentHealth += amount;
        if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;

        OnHealthChanged?.Invoke(CurrentHealth / maxHealth);
    }

    private void StartRegen() {
        if (regenCoroutine != null) return;

        regenCoroutine = StartCoroutine(RegenRoutine());
    }

    private IEnumerator RegenRoutine() {
        while (CurrentHealth < maxHealth && !isDead) {
            yield return new WaitForSeconds(regenRate);
            Heal(regenAmount);
        }
        regenCoroutine = null;
    }

    public void KillPlayer() {
        if (isDead) return;

        isDead = true;
        CurrentHealth = 0;
        OnHealthChanged?.Invoke(0); 

        if (regenCoroutine != null) StopCoroutine(regenCoroutine);

        Debug.Log("Œmieræ gracza!");
        anim.SetTrigger("die");
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence() {
        yield return new WaitForSeconds(0.9f);
        if (gameOverScreen != null) gameOverScreen.enabled = true;
        Time.timeScale = 0;
    }
    public void UpgradeMaxHealth(float amount) {
        maxHealth += amount; 
        CurrentHealth += amount;
        OnHealthChanged?.Invoke(CurrentHealth / maxHealth);
    }

    public void UpgradeRegen(float amount) {
        regenAmount += amount;
    }
}