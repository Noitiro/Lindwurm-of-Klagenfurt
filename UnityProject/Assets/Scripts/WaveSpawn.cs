using System.Collections;
using System.Xml.Xsl;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class WaveSpawn : MonoBehaviour
{   
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject enemyPrefab;
        public int enemyCount;
        public float spawnRate;
    }
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    private float countdown;
    public SpawnState state = SpawnState.COUNTING;
    private float  searchCountdown = 1f;
    public Transform[] spawnpoints;

    void Start()
    {
        countdown = timeBetweenWaves;
    }

    void Update()
    {
        if(state ==SpawnState.WAITING)
        {
            if (!EnemyIsAlive()) 
                {
                WaveCompleted();
            }
            return;
        }
        if (countdown <= 0f)
        {
            if(state != SpawnState.SPAWNING) 
        {
                if (nextWave < waves.Length) 
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }
    void WaveCompleted() {
        Debug.Log("Wave Comleted");
        state = SpawnState.COUNTING;
        countdown = timeBetweenWaves;
        nextWave++;
        if (nextWave >= waves.Length) 
        {
            nextWave = 0;
            Debug.Log("All waves LOOOP");
            ////// TUTAJ KONIEC FALL
        }
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
        }   
        }
        return true;
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        Debug.Log("Spawning wave" + _wave.name);
        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.enemyCount; i++)
        {
            SpawnEnemy(_wave.enemyPrefab);
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }
        state = SpawnState.WAITING;
        yield break;
    }
    void SpawnEnemy (GameObject _enemy)
    {
        Transform _sp = spawnpoints[Random.Range(0, spawnpoints.Length)];
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}

