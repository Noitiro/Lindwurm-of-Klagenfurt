using UnityEngine;
using TMPro;
using System.Collections;

public class WaveUI : MonoBehaviour {
    [Header("Ustawienia")]
    [SerializeField] private WaveSpawn waveSpawner;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Licznik Wrogów")]
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [Tooltip("Ma³y tekst obok licznika, który pokazuje +1 lub -1")]
    [SerializeField] private TextMeshProUGUI enemyDiffText;
    [SerializeField] private string enemyCountPrefix = "Enemies: ";

    [Header("Efekt Powiêkszania")]
    [SerializeField] private float pulseSize = 1.5f;
    [SerializeField] private float returnSpeed = 5f;

    private int previousEnemyCount = 0; 

    private void Start() {
        if (waveSpawner == null) waveSpawner = FindObjectOfType<WaveSpawn>();

        if (waveSpawner != null) {
            waveSpawner.OnWaveChanged += UpdateWaveText;
            UpdateWaveText(waveSpawner.CurrentWaveNumber, waveSpawner.TotalWaves);
        }

        BaseEnemyHealth.OnEnemyCountChanged += UpdateEnemyCountText;

        if (enemyDiffText != null) enemyDiffText.alpha = 0; 
        UpdateEnemyCountText(0);
    }

    private void OnDestroy() {
        if (waveSpawner != null) waveSpawner.OnWaveChanged -= UpdateWaveText;
        BaseEnemyHealth.OnEnemyCountChanged -= UpdateEnemyCountText;
    }

    private void Update() {
        if (waveText != null) {
            waveText.transform.localScale = Vector3.Lerp(waveText.transform.localScale, Vector3.one, Time.deltaTime * returnSpeed);
        }
    }

    private void UpdateWaveText(int currentWave, int totalWaves) {
        if (waveText != null) {
            waveText.text = $"WAVE {currentWave} / {totalWaves}";
            waveText.transform.localScale = Vector3.one * pulseSize;
        }
    }

    private void UpdateEnemyCountText(int currentCount) {
        if (enemyCountText != null) {
            enemyCountText.text = enemyCountPrefix + currentCount.ToString();
        }

        if (enemyDiffText != null) {
            int diff = currentCount - previousEnemyCount;

            if (diff != 0 && previousEnemyCount != 0) {
                if (diff > 0) {
                    enemyDiffText.text = "+" + diff;
                    enemyDiffText.color = Color.red; 
                }
                else {
                    enemyDiffText.text = diff.ToString();
                    enemyDiffText.color = Color.green; 
                }

                StopAllCoroutines(); 
                StartCoroutine(AnimateDiffText());
            }
        }

        previousEnemyCount = currentCount;
    }

    private IEnumerator AnimateDiffText() {
        enemyDiffText.alpha = 1;
        enemyDiffText.transform.localScale = Vector3.one * 1.5f; 

        float duration = 1f;
        float timer = 0f;

        while (timer < duration) {
            timer += Time.deltaTime;
            enemyDiffText.alpha = Mathf.Lerp(1, 0, timer / duration);
            enemyDiffText.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, timer / duration);
            yield return null;
        }

        enemyDiffText.alpha = 0;
    }
}