using UnityEngine;
using TMPro; 

public class DamagePopup : MonoBehaviour {
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private float disappearSpeed = 3f;
    [SerializeField] private float moveYSpeed = 1f;
    [SerializeField] private float lifeTime = 1f;

    private Color textColor;
    private float timer;

    public void Setup(float damageAmount) {
        textMesh.text = damageAmount.ToString("0"); 
        textColor = textMesh.color;
        timer = lifeTime;
    }

    void Update() {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer < 0) {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0) {
                Destroy(gameObject);
            }
        }
    }
}