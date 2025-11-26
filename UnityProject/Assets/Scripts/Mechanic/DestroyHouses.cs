using UnityEngine;

public class DestroyHouses : MonoBehaviour, IDamageable {
//    [SerializeField] Sprite destroyHouseSprite;
//    [SerializeField] bool destroy = false;
//    private SpriteRenderer spriteRenderer;
    private Animator anim;


    private void Awake() {
 //       spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    public void DestroyHouse() {
        anim.SetTrigger("destroy");
    }

    public void Damage(float amount) {
        DestroyHouse();
    }
}