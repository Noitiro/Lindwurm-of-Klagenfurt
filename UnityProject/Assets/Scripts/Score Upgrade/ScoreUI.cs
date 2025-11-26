using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string prefix = "Gold: ";

    [Header("Efekt Powiêkszania")]
    [SerializeField] private float pulseSize = 1.5f;
    [SerializeField] private float returnSpeed = 5f;

    private void Start() {
        Debug.Log("2. ScoreUI startuje..."); 
        if (ScoreManager.Instance != null) {
            Debug.Log("3. ScoreUI ZNALAZ£ Managera!");
            scoreText.text = prefix + ScoreManager.Instance.CurrentScore.ToString();

            ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
        }
        else {
            Debug.LogError("3. ScoreUI NIE ZNALAZ£ Managera (Jest null)!");
        }
    }

    private void OnDestroy() {
        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
        }
    }

    private void Update() {
        scoreText.transform.localScale = Vector3.Lerp(
            scoreText.transform.localScale,
            Vector3.one,
            Time.deltaTime * returnSpeed
        );
    }

    private void UpdateScoreText(int newScore) {
        scoreText.text = prefix + newScore.ToString();

        scoreText.transform.localScale = Vector3.one * pulseSize;
    }
}
