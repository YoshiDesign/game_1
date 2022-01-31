using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour
{
    // The large asteroid's type enum
    const int LARGE_ASTEROID = 6;

    // Different types of asteroids. Does not include large asteroids
    const int ASTEROID_COUNT = 6;

    [SerializeField]
    private int numTerrainRows = 5;

    [SerializeField]
    private float max_distance = 3900.0f;

    [SerializeField]
    private GameObject _terrainRowPrefab;
    private Queue<GameObject> rows = new Queue<GameObject>();
    private GameObject[] rowsAsArray;

    [SerializeField]
    GameObject player;
    //private Player _player;
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

    public int no_enemies;
    GameObject[] gos;

    [SerializeField]
    private GameObject debris;

    [SerializeField]
    private float max_spawn_distance = 3000.0f;

    private Queue<GameObject> debris_array = new Queue<GameObject>();

    [SerializeField]
    private GameObject enemySystem;
    EnemySystem es;

    private bool _isGameOver;

    private void Start()
    {
       // _player = player.GetComponent<Player>();
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

        for (int i = 0; i < numTerrainRows; i++) {
            rows.Enqueue(Instantiate(_terrainRowPrefab, new Vector3(0, 0, (i * 1000)), Quaternion.identity));
        }

        StartCoroutine(SpawnCubes());
        StartCoroutine(SpawnAsteroid());

    }

    private void Update()
    {
        if (rows.Peek() == null) {
            CreateTerrainRow();
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
            else if (spawnLargeAsteroid())
            {
                GameObject clone = Instantiate(largeAsteroid, new Vector3(0, 3000.0f, max_distance), Quaternion.identity);
                _large_asteroid = clone.GetComponent<LargeAsteroid>();
                _large_asteroid.type = v;
            }
 
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }

    }
    /**
     * Spawn a large asteroid when we've spawned a number of every other time
     */
    private bool spawnLargeAsteroid()
    {

        for (int i = 0; i < asteroid_count.Length; i++)
        {
            if (asteroid_count[i] < 5) { 
                return false;
            }
        }

        // Reset the asteroid count
        asteroid_count = new int[ASTEROID_COUNT];

        return true;
    }

    public IEnumerator SpawnCubes()
    {

        for ( ; ; )
        {
            for (int i = 0; i < Random.Range(3, 3); i++)
            {
                for (int j = 0; j < Random.Range(2, 5); j++)
                {
                    debris_array.Enqueue(Instantiate(
                        debris,
                        new Vector3(
                            (float) Random.Range(-2000, 2000),
                            (float) Random.Range(0, 2000),
                            max_spawn_distance + Random.Range(0, 1000)),
                        Quaternion.identity));
                }
            }
            yield return new WaitForSeconds(Random.Range(1, 4));
        }
    }

    public void CreateTerrainRow() {
        // Remove from the queue
        rows.Dequeue();
        // Turn queue into array so we can access the last in line
        rowsAsArray = rows.ToArray();
        // Spawn the next row beginning exactly where the previous last in line left off
        rows.Enqueue(Instantiate(_terrainRowPrefab, new Vector3(0, 0, rowsAsArray[rowsAsArray.Length - 1].transform.position.z + 1000), Quaternion.identity));
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    void OnGui()
    {
        // common GUI code goes here
    }
}
