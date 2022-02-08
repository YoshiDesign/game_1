using UnityEngine;
using UnityEditor;

public class TerrainRow : MonoBehaviour
{

    [SerializeField]
    GameObject followingTerrain;
    [SerializeField]
    GameObject player;
    Player _player;

    public int depth = 20;
    public int width = 1024;
    public int height = 1024;
    public float scale = 22.0f;

    [SerializeField]
    public Vector3 followingStartPosition = new Vector3(0, 0, 9980f);
    public Vector3 proceed_speed = new Vector3(0, 0, -1000.0f);
    public Vector3 thrust_vector = new Vector3(0, 0, 0);
    public float cutoff_distance = -9980.0f;

    void Start()
    {
        _player = player.GetComponent<Player>();
        //terrain = transform.GetComponent<Terrain>();
        //terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    // Update is called once per frame
    void Update()
    {
        proceed();
    }

    void proceed()
    {
        transform.Translate((proceed_speed - _player.thrust) * Time.deltaTime);
        update_grid();
    }

    void update_grid()
    {
        // The tile is behind the player's fov
        if (transform.position.z < cutoff_distance)
        {
            transform.position = followingTerrain.transform.position + followingStartPosition;
        }

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        // terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = x / (float)width * scale;
        float yCoord = y / (float)height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
