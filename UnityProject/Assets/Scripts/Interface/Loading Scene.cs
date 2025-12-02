using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {

    public void LoadMap(string nameMap) {
        SceneManager.LoadScene(nameMap);
        Time.timeScale = 1;
    }

    public void Exit() {
        Application.Quit();
        Debug.Log("Exit");
    }
}
