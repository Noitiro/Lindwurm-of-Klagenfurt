using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string prefix = "Gold: ";

    [Header("Efekt Powiêkszania")]
    [SerializeField] private float pulseSize = 1.5f;
    [SerializeField] private float returnSpeed = 5f;

    private void Start() {
        if (ScoreManager.Instance != null) {
            UpdateScoreText(ScoreManager.Instance.CurrentScore);
            ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
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