using UnityEngine;

public class GameSystem : MonoBehaviour
{

    [SerializeField]
    private GameObject _terrainRowPrefab;
    private GameObject[] rows = new GameObject[3];

    public float cutoff_distance = -1050.0f;
    public Vector3 proceed_speed = new Vector3(0, 0, -350.0f);

    private bool _isGameOver;

    [SerializeField]
    private float max_distance = 2000.0f;

    private void Start()
    {
        Debug.Log("Game Start");

        _isGameOver = false;

        rows[0] = Instantiate(_terrainRowPrefab, new Vector3(0, 0, max_distance), Quaternion.identity);
        rows[1] = Instantiate(_terrainRowPrefab, new Vector3(0, 0, max_distance - 1000.0f), Quaternion.identity);
        rows[2] = Instantiate(_terrainRowPrefab, new Vector3(0, 0, max_distance - 2000.0f), Quaternion.identity);
    }

    void Update()
    {


        rollingMap();
    }

    private void rollingMap() {
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i] == null)
            {
                rows[i] = Instantiate(_terrainRowPrefab, new Vector3(0, 0, max_distance), Quaternion.identity);
            }
        }
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
