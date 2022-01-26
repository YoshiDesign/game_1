using UnityEngine;
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

    public int no_enemies;
    GameObject[] gos;

    [SerializeField]
    private GameObject enemySystem;
    EnemySystem es;

    private Vector3 proceed_speed = new Vector3(0, 0, -350.0f);
    private float cutoff_distance = -1050.0f;
    private bool _isGameOver;

    private void Start()
    {
        Debug.Log("Game Start");

        es = enemySystem.transform.GetComponent<EnemySystem>();

        _isGameOver = false;

        for (int i = 0; i < numRows; i++) {
            rows.Enqueue(Instantiate(_terrainRowPrefab, new Vector3(0, 0, (i * 1000)), Quaternion.identity));
        }

    }

    private void Update()
    {
        if (rows.Peek() == null) {
            CreateTerrainRow();
        }
    }

    public void CreateTerrainRow() {
        rows.Dequeue();
        rows.Enqueue(Instantiate(_terrainRowPrefab, new Vector3(0, 0, max_distance), Quaternion.identity));
        es.SpawnCubes();
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
