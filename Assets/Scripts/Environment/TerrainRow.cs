using UnityEngine;
using UnityEditor;

public class TerrainRow : MonoBehaviour
{

    public int depth = 20;
    public int width = 1024;
    public int height = 1024;
    public float scale = 22.0f;

    public Vector3 proceed_speed = new Vector3(0, 0, -1500.0f);
    public float cutoff_distance = -1050.0f;
    // Start is called before the first frame update
    void Start()
    {
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
        transform.Translate(proceed_speed * Time.deltaTime);
        update_grid();
    }

    void update_grid()
    {
        // The tile is behind the player's fov
        if (transform.position.z < cutoff_distance)
        { 
            Destroy(gameObject);
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
