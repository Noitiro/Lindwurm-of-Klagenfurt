using UnityEngine;

public class CursorManager : MonoBehaviour {
    private void Start() {
        HideCursor();
    }

    public static void HideCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

    public static void ShowCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}