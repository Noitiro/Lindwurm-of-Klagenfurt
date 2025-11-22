using System.Collections;
using UnityEngine; 

public class WaveSpawn : MonoBehaviour {
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
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
    public Transform[] spawnpoints;
    public float timeBetweenWaves = 5f;

    private int nextWave = 0;
    private float countdown;
    public SpawnState state = SpawnState.COUNTING;
    private float searchCountdown = 1f;
    [SerializeField] public PlayerHealth playerHealth;


    void Start() {
        if (spawnpoints.Length == 0) {
            Debug.LogError("Brak przypisanych Spawn Pointów do Wave Spawnera!");
            this.enabled = false;
        }
        countdown = timeBetweenWaves;
    }

    void Update() {
        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive()) {

                WaveCompleted();
            }
            return;
        }

        if (countdown <= 0f) {
            if (state != SpawnState.SPAWNING) {
                if (nextWave < waves.Length) {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
        }
        else {
            countdown -= Time.deltaTime;
        }
    }

    void WaveCompleted() {
        Debug.Log("Fala zakoñczona!");
        state = SpawnState.COUNTING;
        countdown = timeBetweenWaves;

        nextWave++;
        if (nextWave >= waves.Length) {
            Debug.Log("Wszystkie fale ukoñczone! Koniec gry.");

            state = SpawnState.COUNTING;
            this.enabled = false;

            if (playerHealth != null) {
                playerHealth.KillPlayer();
            }
        }
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

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject _enemy) {
        Transform _sp = spawnpoints[Random.Range(0, spawnpoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        Debug.Log("Spawnowanie wroga: " + _enemy.name);
    }
}