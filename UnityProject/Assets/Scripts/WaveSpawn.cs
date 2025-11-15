using UnityEngine;
using System.Collections;
using UnityEditorInternal;

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
    public float countdown;
    public SpawnState state = SpawnState.COUNTING;
    
    void Start()
    {
        countdown = timeBetweenWaves;
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            if(State )
        }
    }
}

