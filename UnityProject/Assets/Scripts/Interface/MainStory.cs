using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    private void OnEnable() {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
