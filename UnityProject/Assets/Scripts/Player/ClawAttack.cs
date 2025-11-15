using UnityEngine;
using System.Collections;

public class ClawAttack : MonoBehaviour {
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;

    [SerializeField] private float attackCooldown = 1f;   
    private bool canAttack = true;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clawSound;

    PlayerController playerController;
    private RaycastHit2D[] hits;

    private void Awake()
    {
        playerController = new PlayerController();
    }
    private void OnEnable() => playerController.Enable();
    private void OnDisable() => playerController.Disable();

    private void Update()
    {
        if(playerController.Player.ClawAttack.WasPressedThisFrame() && canAttack)
        {
            StartCoroutine(PerfomAttack());
        }
    }

    private void Attack()
    {
        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            IDamageable damageable = hits[i].collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damageAmount);
            }
        }
    }

    private IEnumerator PerfomAttack()
    {
        canAttack = false;
        if (clawSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clawSound);
        }
        Attack();
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
}
