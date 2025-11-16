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


    [Header("Referencje do skryptów Ataków")]
    [SerializeField] private ClawAttack clawAttack;
    // [SerializeField] private IceAttack iceAttack;
    // [SerializeField] private TailAttack tailAttack;
    // [SerializeField] private FireBreathAttack fireBreathAttack;

    private PlayerController playerController;

    public static event Action<AttackType> OnAttackSelected;

    private void Awake() {
        playerController = new PlayerController();

        
        //if (clawAttack == null || iceAttack == null || tailAttack == null || FireBreathAttack == null) {
        //    Debug.LogError("Nie wszystkie skrypty ataków s¹ przypisane do AttackSelector!");
        //    this.enabled = false;
        //}
    }

    private void OnEnable() {
        playerController.Enable();

        // klawisze 1-4
        playerController.Player.SelectAttack1.performed += ctx => SelectAttack(AttackType.Claw);
        playerController.Player.SelectAttack2.performed += ctx => SelectAttack(AttackType.Ice);
        playerController.Player.SelectAttack3.performed += ctx => SelectAttack(AttackType.TailSwipe);
        playerController.Player.SelectAttack4.performed += ctx => SelectAttack(AttackType.FireBreath);

        // myszka
        playerController.Player.Attack.performed += HandleAttackExecution;
    }

    private void OnDisable() {
        playerController.Disable();
        playerController.Player.SelectAttack1.performed -= ctx => SelectAttack(AttackType.Claw);
        playerController.Player.SelectAttack2.performed -= ctx => SelectAttack(AttackType.Ice);
        playerController.Player.SelectAttack3.performed -= ctx => SelectAttack(AttackType.TailSwipe);
        playerController.Player.SelectAttack4.performed -= ctx => SelectAttack(AttackType.FireBreath);
        playerController.Player.Attack.performed -= HandleAttackExecution;
    }

    private void SelectAttack(AttackType newAttack) {
    
        if (currentState != PlayerState.Idle) return;

        currentAttackType = newAttack;
        Debug.Log("Wybrano atak: " + newAttack);
        OnAttackSelected?.Invoke(newAttack);
    }

    private void HandleAttackExecution(InputAction.CallbackContext context) {
        if (currentState == PlayerState.Idle) {
            SetState(PlayerState.Attacking);

            switch (currentAttackType) {
                case AttackType.Claw:
                    if (clawAttack.IsReady()) {
                        SetState(PlayerState.Attacking); // Ustaw GLOBALN¥ blokadê
                        clawAttack.ExecuteAttack(this);  // Uruchom atak
                    }
                    else {
                        Debug.Log("Atak Pazurami jest na Cooldownie!");
                        SetState(PlayerState.Idle); // Wa¿ne: Zwolnij blokadê, jeœli atak nie jest gotowy
                    }
                    break;

                case AttackType.Ice:
                    Debug.Log("Atak 'TailSwipe' nie jest jeszcze pod³¹czony.");
                    SetState(PlayerState.Idle); 
                    break;

                case AttackType.TailSwipe:
                    Debug.Log("Atak 'TailSwipe' nie jest jeszcze pod³¹czony.");
                    SetState(PlayerState.Idle); 
                    break;

                case AttackType.FireBreath:
                    Debug.Log("Atak 'FireBreath' nie jest jeszcze pod³¹czony.");
                    SetState(PlayerState.Idle); 
                    break;
            }
        }
    }

    // --- Metoda Publiczna (dla skryptów ataków) ---

    // Twoje skrypty (ClawAttack, IceAttack) MUSZ¥ wywo³aæ tê funkcjê,
    // gdy zakoñcz¹ swój cooldown, aby "zwolniæ" gracza.
    public void SetState(PlayerState newState) {
        currentState = newState;
        Debug.Log("Nowy stan gracza: " + newState);
    }
}