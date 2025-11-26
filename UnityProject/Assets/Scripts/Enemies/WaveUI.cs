using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour {
    [Header("Ustawienia")]
    [SerializeField] private WaveSpawn waveSpawner;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Efekt Powiêkszania")]
    [SerializeField] private float pulseSize = 1.5f;
    [SerializeField] private float returnSpeed = 5f;

    private void Start() {
        if (waveSpawner == null) {
            waveSpawner = FindObjectOfType<WaveSpawn>();
        }

        if (waveSpawner != null) {
            waveSpawner.OnWaveChanged += UpdateWaveText;
            UpdateWaveText(waveSpawner.CurrentWaveNumber, waveSpawner.TotalWaves);
        }
    }

    private void OnDestroy() {
        if (waveSpawner != null) {
            waveSpawner.OnWaveChanged -= UpdateWaveText;
        }
    }

    private void Update() {
        if (waveText != null) {
            waveText.transform.localScale = Vector3.Lerp(
                waveText.transform.localScale,
                Vector3.one,
                Time.deltaTime * returnSpeed
            );
        }
    }

    private void UpdateWaveText(int currentWave, int totalWaves) {
        Debug.Log($"UI PRÓBUJE NAPISAÆ: WAVE {currentWave} / {totalWaves}");
        if (waveText != null) {
            waveText.text = $"WAVE {currentWave} / {totalWaves}";
            waveText.transform.localScale = Vector3.one * pulseSize;
        }
    }
}