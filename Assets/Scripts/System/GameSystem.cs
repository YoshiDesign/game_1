using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour
{
    [SerializeField]
    private int numRows = 5;

    [SerializeField]
    private float max_distance = 3900.0f;

    [SerializeField]
    private GameObject _terrainRowPrefab;
    private Queue<GameObject> rows = new Queue<GameObject>();

    [SerializeField]
    GameObject player;
    private Player _player;

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
    [SerializeField]
    private GameObject asteroid_7;
    private GameObject[] asteroids;

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

    private Vector3 proceed_speed = new Vector3(0, 0, -350.0f);
    private float cutoff_distance = -1050.0f;
    private bool _isGameOver;

    private void Start()
    {
        _player = player.GetComponent<Player>();
        es = enemySystem.transform.GetComponent<EnemySystem>();

        asteroids = new GameObject[] {
            asteroid_1,
            asteroid_2,
            asteroid_3,
            asteroid_4,
            asteroid_5,
            asteroid_6,
            asteroid_7
        };

        _isGameOver = false;

        for (int i = 0; i < numRows; i++) {
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
        for ( ; ; )
        { 
            int v = Random.Range(0, 6);
            Instantiate(asteroids[v], new Vector3(0, 3000.0f, max_distance), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0, 3));
        }

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
        rows.Dequeue();
        rows.Enqueue(Instantiate(_terrainRowPrefab, new Vector3(0, 0, max_distance), Quaternion.identity));
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        no_enemies = gos.Length;
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
