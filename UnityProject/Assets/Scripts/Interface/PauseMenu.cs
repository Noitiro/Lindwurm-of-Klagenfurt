using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private Canvas gameOverScreen;
    bool menuPause;

    private void Awake() {
        playerController = new PlayerController();
    }
    private void Start() {
        menuPause = false;
    }

    private void OnEnable() {
        playerController.Enable();
    }

    private void OnDisable() {
        playerController.Disable();
    }

    private void Update() {
        playerController.UI.Menu.performed += context => {
            PauseMenuShow();
        };
    }

    public void PauseMenuShow() {

            if(menuPause == false) {
                menuPause =true;
                if (gameOverScreen != null) gameOverScreen.enabled = true;
                Time.timeScale = 0;
                CursorManager.ShowCursor();

            } else {
                menuPause=false;
                if (gameOverScreen != null) gameOverScreen.enabled = false;
                Time.timeScale = 1;
                CursorManager.HideCursor();
            }
    }
}
