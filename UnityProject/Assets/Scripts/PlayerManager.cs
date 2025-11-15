using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HeaderAttribute("Speed Player")]

    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float speed = 2f;

    public float Speed { get => speed; set => speed = value; }
    public float NormalSpeed { get => normalSpeed; set => normalSpeed = value; }
    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
}
