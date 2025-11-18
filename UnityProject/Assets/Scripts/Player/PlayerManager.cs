using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HeaderAttribute("Speed Player")]

    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float speed = 2f;

    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy = 100f;
    [SerializeField] private float sprintCostPerSecond = 20f;
    [SerializeField] private float regenPerSecond = 10f;

    public float Speed { get => speed; set => speed = value; }
    public float NormalSpeed { get => normalSpeed; set => normalSpeed = value; }
    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }

    public void UpdateEnergy(bool isSprinting, float deltaTime) {
        if (isSprinting && currentEnergy > 0f) {
            currentEnergy -= sprintCostPerSecond * deltaTime;
            if (currentEnergy <= 0f) {
                currentEnergy = 0f;
                Speed = NormalSpeed;
            }
        }
        else {
            currentEnergy += regenPerSecond * deltaTime;
            if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        }
    }
}
