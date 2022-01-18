using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_Quadrant : MonoBehaviour
{

    public int depth = 20;
    public int width = 1000;
    public int height = 1000;
    public float scale = 1.0f;

    private Terrain terrain;
    public GameObject player;
    public Vector3 proceed_speed = new Vector3(0, 0, -350.0f);
    public float cutoff_distance = -1050.0f;
    public float update_cooldown = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    // Update is called once per frame
    void Update()
    {
        proceed();
    }

    public void setup(int Q)
    {

    }

    void proceed()
    {
        // Map constantly moves toward the player
        transform.Translate(proceed_speed * Time.deltaTime);
        // Check if terrain tiles need to be destroyed & reinstantiated
        update_grid();
    }

    public void proceed_side(string side)
    {
        Debug.Log("Hit " + side + " side!");
    }

    void update_grid()
    {
        // The tile is behind the player's fov
        if (transform.position.z < cutoff_distance)
        {
            Instantiate(gameObject, transform.position + new Vector3(0, 0, 3000.0f), Quaternion.identity);
            Destroy(gameObject);
        }

        // The tile is more than 2000m to either side of the player
        if (Mathf.Abs(transform.position.x - player.transform.position.x) > 2000.0f)
        {
            float mod = 1.0f;
            if (transform.position.x - player.transform.position.x < 0) mod = -1.0f;
            //Instantiate(gameObject, transform.position + new Vector3(mod * 2000.0f, 0, 0), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (x == width || y == width)
                {
                    heights[x,y] = 5.0f;
                }
                else { 
                    heights[x, y] = CalculateHeight(x, y);
                    if (x == 7 && y == 4)
                        Debug.Log("height: " + heights[x, y]);
                }
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
