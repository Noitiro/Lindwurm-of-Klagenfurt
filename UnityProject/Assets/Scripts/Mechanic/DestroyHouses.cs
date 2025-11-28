using UnityEngine;

public class DestroyHouses : MonoBehaviour, IDamageable {

    [Header("Wytrzyma³osc")]
    [SerializeField] private int hitsToDestroy = 3;
    private int currentHits = 0;

    [Header("Efekty Wizualne")]
    [SerializeField] private Color burntColor = new Color(0.4f, 0.4f, 0.4f);

    [Header("Nagroda")]
    [SerializeField] private int scoreValue = 2; 

    private bool isDestroyed = false;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Burn() {
        if (isDestroyed) return;

        currentHits++;
        Debug.Log($"Domek podpalamy! ({currentHits}/{hitsToDestroy})");

        if (spriteRenderer != null) {
            float burnPercent = (float)currentHits / hitsToDestroy;
            spriteRenderer.color = Color.Lerp(Color.white, burntColor, burnPercent);
        }

        if (currentHits >= hitsToDestroy) {
            DestroyHouse();
        }
    }

    public void Damage(float amount) { }

    public void DestroyHouse() {
        if (isDestroyed) return;

        isDestroyed = true;
        if (spriteRenderer != null) spriteRenderer.color = Color.white;

        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        anim.SetTrigger("destroy");
    }

    public void OnDestroyAnimationFinished() {
        Destroy(gameObject);
    }
}