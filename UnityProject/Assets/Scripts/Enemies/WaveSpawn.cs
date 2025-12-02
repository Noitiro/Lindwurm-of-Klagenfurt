using System.Collections;
using UnityEngine;
using System;

public class WaveSpawn : MonoBehaviour {
    public enum SpawnState { SPAWNING, WAITING, SHOPPING };

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

        StartCoroutine(StartFirstWave());
    }

    IEnumerator StartFirstWave() {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    void Update() {
        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive()) {
                WaveCompleted();
            }
        }
    }

    void WaveCompleted() {
        Debug.Log("Fala zakoñczona! Otwieram sklep.");
        state = SpawnState.SHOPPING;

        if (UpgradeManager.Instance != null) {
            UpgradeManager.Instance.OpenUpgradeMenu();
        }
        else {
            NextWave();
        }
    }

    public void NextWave() {
        nextWave++;

        if (nextWave >= waves.Length) {
            Debug.Log("Koniec gry - wszystkie fale pokonane!");

            this.enabled = false;
            return;
        }

        UpdateWaveUI();
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

        System.Collections.Generic.List<GameObject> enemiesToSpawn = new System.Collections.Generic.List<GameObject>();

        foreach (Wave.EnemyGroup group in _wave.enemyGroups) {
            for (int i = 0; i < group.count; i++) {
                enemiesToSpawn.Add(group.enemyPrefab);
            }
        }
        while (enemiesToSpawn.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, enemiesToSpawn.Count);
            GameObject selectedEnemy = enemiesToSpawn[randomIndex];

            SpawnEnemy(selectedEnemy);

            enemiesToSpawn.RemoveAt(randomIndex);

            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        state = SpawnState.WAITING;
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