using System.Collections;
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

    public void StartGame(string nameMap) {
        StartCoroutine(WaitSecond(2, nameMap));
    }
    private IEnumerator WaitSecond(float time, string nameMap) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(nameMap);
    }
}
