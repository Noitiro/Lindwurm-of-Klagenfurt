using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class AttackSelector : MonoBehaviour {
    public enum AttackType {
        Claw,
        Ice,
        //TailSwipe,
        FireBreath
    }

    public enum PlayerState { Idle, Attacking }

    [Header("Monitor Stanu")]
    [SerializeField] public AttackType CurrentAttackType = AttackType.Claw;

    [SerializeField] private PlayerState currentState = PlayerState.Idle;


    [Header("Referencje do skrypt�w Atak�w")]
    [SerializeField] private ClawAttack clawAttack;
    [SerializeField] private IceAttack iceAttack;
    //[SerializeField] private TailAttack tailAttack;
    [SerializeField] private FireBreathAttack fireBreathAttack;
    public ClawAttack ClawAttackScript { get { return clawAttack; } }
    public IceAttack IceAttackScript { get { return iceAttack; } }
    public FireBreathAttack FireBreathAttackScript { get { return fireBreathAttack; } }
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
        Vector2 input = context.ReadValue<Vector2>();
        float scrollValue = 0f;
        if (input.y != 0) {
            scrollValue = input.y;
        }
        else if (input.x != 0) {
            scrollValue = input.x;
        }
        if (scrollValue == 0) return;

        int currentIndex = (int)CurrentAttackType;
        int attackCount = Enum.GetNames(typeof(AttackType)).Length;

        if (scrollValue > 0) { 
            currentIndex++;
            if (currentIndex >= attackCount) {
                currentIndex = 0;
            }
        }
        else if (scrollValue < 0) { 
            currentIndex--;
            if (currentIndex < 0) {
                currentIndex = attackCount - 1;
            }
        }

        SelectAttack((AttackType)currentIndex);
    }
    private void SelectAttack(AttackType newAttack) {

        if (currentState != PlayerState.Idle) return;

        CurrentAttackType = newAttack;
        Debug.Log("Wybrano atak: " + newAttack);
        OnAttackSelected?.Invoke(newAttack);
    }

    private void HandleAttackExecution(InputAction.CallbackContext context) {
        if (currentState == PlayerState.Idle) {

            switch (CurrentAttackType) {
                case AttackType.Claw:
                    if (clawAttack != null && clawAttack.IsReady()) {
                        SetState(PlayerState.Attacking);
                        clawAttack.ExecuteAttack(this);
                    } else {
                        Debug.Log("Atak Pazurami jest na Cooldownie lub niepod��czony!");
                    }
                    break;

                case AttackType.Ice:
                    if (iceAttack != null && iceAttack.IsReady()) {
                        SetState(PlayerState.Attacking);
                        iceAttack.ExecuteAttack(this);
                    }
                    else {
                        Debug.Log("Atak Lodem jest na Cooldownie lub niepod��czony!");
                    }
                    break;
                //case AttackType.TailSwipe:
                //    Debug.Log("Atak 'TailSwipe' nie jest jeszcze pod��czony.");
                //    SetState(PlayerState.Idle);
                //    break;

                case AttackType.FireBreath:
                    if (fireBreathAttack != null && fireBreathAttack.IsReady()) {
                        SetState(PlayerState.Attacking);
                        fireBreathAttack.ExecuteAttack(this);
                    }
                    else {
                        Debug.Log("Atak Ogniem jest na Cooldownie lub niepod��czony!");
                    }
                    break;
            }
        }
    }
    public void SetState(PlayerState newState) {
        currentState = newState;
        Debug.Log("Nowy stan gracza: " + newState);
    }
}