using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {

    public void LoadMap() {
        SceneManager.LoadScene(1);
    }

    public void Exit() {
        Application.Quit();
        Debug.Log("Exit");
    }
}
