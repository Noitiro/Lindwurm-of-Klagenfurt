using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    private void OnEnable() {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
