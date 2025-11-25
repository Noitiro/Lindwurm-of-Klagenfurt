using UnityEngine;

public class DestroyHouses : MonoBehaviour, IDamageable {
    [SerializeField] Sprite destroyHouseSprite;
    [SerializeField] bool destroy = false;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroyHouse() {
            spriteRenderer.sprite = destroyHouseSprite;
    }

    public void Damage(float amount) {
        DestroyHouse();
    }
}