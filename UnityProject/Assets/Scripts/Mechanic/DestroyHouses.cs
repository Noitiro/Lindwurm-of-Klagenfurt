using UnityEngine;

public class DestroyHouses : MonoBehaviour, IDamageable {
    public bool destroy = false;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void DestroyHouse() {
        anim.SetTrigger("destroy");
        destroy = true;
    }

    public void Damage(float amount) {
        DestroyHouse();
    }
}