using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class AttackSelector : MonoBehaviour {
    public enum AttackType {
        Claw,
        Ice,
        TailSwipe,
        FireBreath
    }

    public enum PlayerState { Idle, Attacking }

    [Header("Monitor Stanu")]
    [SerializeField] private AttackType currentAttackType = AttackType.Claw;

    [SerializeField] private PlayerState currentState = PlayerState.Idle;


    [Header("Referencje do skrypt�w Atak�w")]
    [SerializeField] private ClawAttack clawAttack;
    // [SerializeField] private IceAttack iceAttack;
    // [SerializeField] private TailAttack tailAttack;
    // [SerializeField] private FireBreathAttack fireBreathAttack;

    private PlayerController playerController;

    public static event Action<AttackType> OnAttackSelected;

    private void Awake() {
        playerController = new PlayerController();
    }

    private void OnEnable() {
        playerController.Enable();

        playerController.Player.ChangeAttack.performed += HandleScroll;

        playerController.Player.Attack.performed += HandleAttackExecution;
    }

    private void OnDisable() {
        playerController.Disable();

        playerController.Player.ChangeAttack.performed -= HandleScroll;

        playerController.Player.Attack.performed -= HandleAttackExecution;
    }
    private void HandleScroll(InputAction.CallbackContext context) {
        float scrollValue = context.ReadValue<Vector2>().y;

        if (scrollValue == 0) return;

        int currentIndex = (int)currentAttackType;
        int attackCount = Enum.GetNames(typeof(AttackType)).Length;

        if (scrollValue > 0) {
            currentIndex++;
            if (currentIndex >= attackCount) {
                currentIndex = 0;
            }
        } else if (scrollValue < 0) {
            currentIndex--;
            if (currentIndex < 0) {
                currentIndex = attackCount - 1;
            }
        }

        SelectAttack((AttackType)currentIndex);
    }
    private void SelectAttack(AttackType newAttack) {

        if (currentState != PlayerState.Idle) return;

        currentAttackType = newAttack;
        Debug.Log("Wybrano atak: " + newAttack);
        OnAttackSelected?.Invoke(newAttack);
    }

    private void HandleAttackExecution(InputAction.CallbackContext context) {
        if (currentState == PlayerState.Idle) {
            switch (currentAttackType) {
                case AttackType.Claw:
                    if (clawAttack != null && clawAttack.IsReady()) {
                        SetState(PlayerState.Attacking);
                        clawAttack.ExecuteAttack(this);
                    } else {
                        Debug.Log("Atak Pazurami jest na Cooldownie lub niepod��czony!");
                    }
                    break;

                case AttackType.Ice:
                    Debug.Log("Atak 'TailSwipe' nie jest jeszcze pod��czony.");
                    SetState(PlayerState.Idle);
                    break;

                case AttackType.TailSwipe:
                    Debug.Log("Atak 'TailSwipe' nie jest jeszcze pod��czony.");
                    SetState(PlayerState.Idle);
                    break;

                case AttackType.FireBreath:
                    Debug.Log("Atak 'FireBreath' nie jest jeszcze pod��czony.");
                    SetState(PlayerState.Idle);
                    break;
            }
        }
    }
    public void SetState(PlayerState newState) {
        currentState = newState;
        Debug.Log("Nowy stan gracza: " + newState);
    }
}