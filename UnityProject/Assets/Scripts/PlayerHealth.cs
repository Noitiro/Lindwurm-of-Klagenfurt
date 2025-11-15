using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float startingHealth = 100;
    public float currentHealth { get; private set; }
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    public void Damage(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log("Player damage: " + damage);

        if (currentHealth > 0)
            anim.SetTrigger("hurt");
        else
            anim.SetTrigger("die");
    }
}
