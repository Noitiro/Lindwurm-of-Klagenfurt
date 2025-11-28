using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

public class AttackSelector : MonoBehaviour {
    public enum AttackType {
        Claw,
        Ice,
        FireBreath,
        TailSwipe
    }
    public enum EnemyType {
        Normal,
        Ice,
        Armored 
    }
    private void Start() {
        EnablePreview(CurrentAttackType);
    }

    public enum PlayerState { Idle, Attacking }

    [Header("Monitor StanuS")]
    [SerializeField] public AttackType CurrentAttackType = AttackType.Claw;

    [SerializeField] private PlayerState currentState = PlayerState.Idle;


    [Header("Referencje do skrypt�w Atak�w")]
    [SerializeField] private ClawAttack clawAttack;
    [SerializeField] private IceAttack iceAttack;
    [SerializeField] private TailAttack tailAttack;
    [SerializeField] private FireBreathAttack fireBreathAttack;
    public ClawAttack ClawAttackScript { get { return clawAttack; } }
    public IceAttack IceAttackScript { get { return iceAttack; } }
    public TailAttack TailAttackScript { get { return tailAttack; } }
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
            scrollValue = -input.x;
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

        DisablePreview(CurrentAttackType);

        CurrentAttackType = newAttack;
        Debug.Log("Wybrano atak: " + newAttack);
        OnAttackSelected?.Invoke(newAttack);

        EnablePreview(CurrentAttackType);
    }

    private void HandleAttackExecution(InputAction.CallbackContext context) {
        if (EventSystem.current.IsPointerOverGameObject()) return;
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
                case AttackType.TailSwipe:
                    if (tailAttack != null && tailAttack.IsReady()) {
                        SetState(PlayerState.Attacking);
                        tailAttack.ExecuteAttack(this);
                    }
                    else {
                        Debug.Log("Atak Ogonem jest na Cooldownie!");
                    }
                    break;

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
    }
    private void EnablePreview(AttackType type) {
        switch (type) {
            case AttackType.Claw: if (clawAttack) clawAttack.TogglePreview(true); break;
            case AttackType.Ice: if (iceAttack) iceAttack.TogglePreview(true); break;
            case AttackType.FireBreath: if (fireBreathAttack) fireBreathAttack.TogglePreview(true); break;
            case AttackType.TailSwipe: if (tailAttack) tailAttack.TogglePreview(true); break;
        }
    }

    private void DisablePreview(AttackType type) {
        switch (type) {
            case AttackType.Claw: if (clawAttack) clawAttack.TogglePreview(false); break;
            case AttackType.Ice: if (iceAttack) iceAttack.TogglePreview(false); break;
            case AttackType.FireBreath: if (fireBreathAttack) fireBreathAttack.TogglePreview(false); break;
            case AttackType.TailSwipe: if (tailAttack) tailAttack.TogglePreview(false); break;
        }
    }
}