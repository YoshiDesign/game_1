using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour
{
    /**
     * Constant things
     */
    const int SPACE = 1;
    // The large asteroid's type enum
    const int LARGE_ASTEROID = 6;
    // Different types of asteroids. Does not include large asteroids
    const int ASTEROID_COUNT = 6;
    [SerializeField]
    private float max_spawn_distance = 3000.0f;
    [SerializeField]
    private float max_distance = 3900.0f;
    [SerializeField]
    private int numTerrainRows = 5;

    /**
     * Game state things
     */
    private bool _isGameOver;
    private int current_level;
    [SerializeField]
    private int n_player_locks;     // Number of enemies the player has a locked-on
    Vector3 rng_vec = new Vector3(0, 0, 0);

    /**
     * Environment things
     */
    [SerializeField]
    private GameObject debris;
    private GameObject[] rowsAsArray;

    /**
     * Asteroid things
     */
    Asteroid _asteroid;
    LargeAsteroid _large_asteroid;
    [SerializeField]
    private GameObject largeAsteroid;
    [SerializeField]
    private GameObject asteroid_1;
    [SerializeField]
    private GameObject asteroid_2;
    [SerializeField]
    private GameObject asteroid_3;
    [SerializeField]
    private GameObject asteroid_4;
    [SerializeField]
    private GameObject asteroid_5;
    [SerializeField]
    private GameObject asteroid_6;
    // Array of asteroid prefabs
    private GameObject[] asteroids;
    // Tracking instantiation
    private int[] asteroid_count = new int[ASTEROID_COUNT];

    /**
     * Entity things
     */
    [SerializeField]
    GameObject player;
    private Player _player;
    [SerializeField]
    private GameObject enemySystem;
    EnemySystem es;
    [SerializeField]
    private GameObject homingMisslePowerup_prefab;

    private void Start()
    {
        current_level = SPACE;

        _player = player.GetComponent<Player>();

        es = enemySystem.transform.GetComponent<EnemySystem>();

        asteroids = new GameObject[] {
            asteroid_1,
            asteroid_2,
            asteroid_3,
            asteroid_4,
            asteroid_5,
            asteroid_6
        };

        _isGameOver = false;

        // Begin space debris
        if (current_level == SPACE) {
            for (int i = 0; i < Random.Range(5, 10); i++)
            {
                for (int j = 0; j < Random.Range(10, 15); j++)
                {
                    Instantiate(
                        debris,
                        new Vector3(
                            (float)Random.Range(-8000, 8000),
                            (float)Random.Range(0, 2000),
                            max_spawn_distance + Random.Range(0, 5000)),
                        Quaternion.identity);
                }
            }
        }

        StartCoroutine(SpawnAsteroid());
        StartCoroutine(SpawnPowerup());
        StartCoroutine(spawnLargeAsteroid());

    }

    private void Update()
    {
        //if (rows.Peek() == null) {
        //    CreateTerrainRow();
        //}
    }

    public IEnumerator SpawnPowerup()
    {
        for ( ; ; ) {
            yield return new WaitForSeconds(5);
            rng_vec.x = Random.Range(-1800, 1800);
            rng_vec.y = Random.Range(20, 1000);
            rng_vec.z = max_distance;
            Instantiate(homingMisslePowerup_prefab, rng_vec, Quaternion.identity);
        }
    } 
    public IEnumerator SpawnAsteroid()
    {
        /**
         * TODO - Enumerate asteroids as constants
         */ 

        for ( ; ; )
        { 
            // RNG - asteroid type
            int v = Random.Range(0, ASTEROID_COUNT + 1);

            // Increment to 10
            if (v < LARGE_ASTEROID) // Within the array of small asteroids
            {
                asteroid_count[v] = asteroid_count[v] < 10 ? asteroid_count[v] + 1 : asteroid_count[v];
                GameObject clone = Instantiate(asteroids[v], new Vector3(0, 3000.0f, max_distance), Quaternion.identity);
                _asteroid = clone.GetComponent<Asteroid>();
                _asteroid.type = v;
            }
 
            yield return new WaitForSeconds(Random.Range(2, 5));
        }

    }
    /**
     * Spawn a large asteroid when we've spawned a number of every other time
     */
    private IEnumerator spawnLargeAsteroid()
    {
        for (; ; ) { 
            GameObject clone = Instantiate(largeAsteroid, new Vector3(0, 3000.0f, max_distance), Quaternion.identity);
            _large_asteroid = clone.GetComponent<LargeAsteroid>();
            yield return new WaitForSeconds(Random.Range(8, 35));
        }

    }

    //public void CreateTerrainRow() {
    //    // Remove from the queue
    //    rows.Dequeue();
    //    // Turn queue into array so we can access the last in line
    //    rowsAsArray = rows.ToArray();
    //    // Spawn the next row beginning exactly where the previous last in line left off
    //    rows.Enqueue(Instantiate(_terrainRowPrefab, new Vector3(0, 0, rowsAsArray[rowsAsArray.Length - 1].transform.position.z + 1000), Quaternion.identity));
    //}

    public void GameOver()
    {
        _isGameOver = true;
    }

}
