using UnityEngine;

public class EnemiesFollows : MonoBehaviour {
    
    [SerializeField] private float speed = 2f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    public void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {
        
    }

    void Update() {
        
    }
    
}
