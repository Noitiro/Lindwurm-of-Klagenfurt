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

    public void StartTime(float time) {
        StartCoroutine(WaitSecond(time));
    }
    private IEnumerator WaitSecond(float time) {
        yield return new WaitForSeconds(time);
    }
}
