using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HeaderAttribute("Speed Player")]

    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float speed = 3f;

    [HeaderAttribute("Health Player")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float currentHealth;

    public float Speed { get => speed; set => speed = value; }
    public float NormalSpeed { get => normalSpeed; set => normalSpeed = value; }
    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
    public float StartingHealth { get => startingHealth; set => startingHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
}
