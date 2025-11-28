using System.Collections;
using UnityEngine;
using System;

public class WaveSpawn : MonoBehaviour {
    public enum SpawnState { SPAWNING, WAITING, SHOPPING }; // Zmieniliœmy nazwê stanu na SHOPPING

    [System.Serializable]
    public class Wave {
        public string name;
        [System.Serializable]
        public class EnemyGroup {
            public GameObject enemyPrefab;
            public int count;
        }
        public EnemyGroup[] enemyGroups;
        public float spawnRate;
    }

    public Wave[] waves;
    public int CurrentWaveNumber => nextWave + 1;
    public int TotalWaves => waves.Length;
    public Transform[] spawnpoints;
    public float timeBetweenWaves = 5f;

    private int nextWave = 0;

    // Zmieniamy domyœlny stan na WAITING, ¿eby Update nie szala³ na starcie
    public SpawnState state = SpawnState.WAITING;
    private float searchCountdown = 1f;

    [SerializeField] public PlayerHealth playerHealth;

    public event Action<int, int> OnWaveChanged;

    void Start() {
        BaseEnemyHealth.EnemiesAliveCount = 0;
        if (spawnpoints.Length == 0) {
            Debug.LogError("Brak przypisanych Spawn Pointów do Wave Spawnera!");
            this.enabled = false;
            return;
        }

        UpdateWaveUI();

        // --- NAPRAWA: Rêczne uruchomienie pierwszej fali ---
        // Uruchamiamy falê 0 z opóŸnieniem timeBetweenWaves
        StartCoroutine(StartFirstWave());
    }

    IEnumerator StartFirstWave() {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    void Update() {
        // Update sprawdza TYLKO czy wrogowie ¿yj¹.
        // Nie odlicza czasu do nastêpnej fali, bo tym steruje Sklep.
        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive()) {
                WaveCompleted();
            }
        }
    }

    void WaveCompleted() {
        Debug.Log("Fala zakoñczona! Otwieram sklep.");

        // Ustawiamy stan na "Zakupy" - w tym stanie Update nic nie robi
        state = SpawnState.SHOPPING;

        if (UpgradeManager.Instance != null) {
            UpgradeManager.Instance.OpenUpgradeMenu();
        }
        else {
            // Jeœli nie ma managera (np. testy), idŸ dalej automatycznie
            NextWave();
        }
    }

    // Ta funkcja jest wywo³ywana przez przycisk w Sklepie (UpgradeManager)
    public void NextWave() {
        nextWave++;

        if (nextWave >= waves.Length) {
            Debug.Log("Koniec gry - wszystkie fale pokonane!");
            // Tutaj mo¿esz dodaæ logikê wygranej
            // nextWave = 0; // Lub zapêtlenie
            this.enabled = false;
            return;
        }

        UpdateWaveUI(); // Aktualizuj tekst UI
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f) {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave) {
        Debug.Log("Spawnowanie fali: " + _wave.name);
        state = SpawnState.SPAWNING;

        foreach (Wave.EnemyGroup group in _wave.enemyGroups) {
            for (int i = 0; i < group.count; i++) {
                SpawnEnemy(group.enemyPrefab);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        }

        state = SpawnState.WAITING; // Teraz czekamy a¿ gracz wszystkich pokona
        yield break;
    }

    void SpawnEnemy(GameObject _enemy) {
        Transform _sp = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    private void UpdateWaveUI() {
        int displayWave = nextWave + 1;
        if (displayWave > waves.Length) displayWave = waves.Length;
        OnWaveChanged?.Invoke(displayWave, waves.Length);
    }
}