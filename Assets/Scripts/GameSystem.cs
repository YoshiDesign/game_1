using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public int cols = 4;
    public int rows = 3;

    public GameObject tq_prefab;
    private Dictionary<string, (int, int)> quadrant_lookup = new Dictionary<string, (int, int)>();
    public GameObject[,] grid = new GameObject[3, 4];

    private void Start()
    {
        Debug.Log("GameSystem is online.");

        // Build the grid of Terrain_Quadrants
        for (int i = 0; i < rows; i++) {

            for (int j = 0; j < rows; j++) {

                float offset_left = 0.0f;
                float offset_zed = 0.0f;
                switch (j)
                {
                    case 0: offset_left = -2000.0f; break; // Far
                    case 1: offset_left = -1000.0f; break;
                    case 2: offset_left = 0.0f;     break;
                    case 3: offset_left = 1000.0f;  break; // Far right
                }

                switch (i)
                {
                    case 0: offset_zed = 2000.0f; break;   // Furthest away
                    case 1: offset_zed = 1000.0f; break;
                    case 2: offset_zed = 0.0f;    break;
                }

                grid[i,j] = Instantiate(tq_prefab, new Vector3(offset_left, 0.0f, offset_zed), Quaternion.identity);
            }

        }

        for (int i = 0; i < rows; i++)
        {
            Debug.Log(grid[i,0].GetInstanceID());
        }

        // Create a lookup table
        quadrant_lookup["q1"] = (0, 3);
        quadrant_lookup["q2"] = (0, 2);
        quadrant_lookup["q3"] = (0, 1);
        quadrant_lookup["q4"] = (0, 0);
        quadrant_lookup["q5"] = (1, 3);
        quadrant_lookup["q6"] = (1, 2);
        quadrant_lookup["q7"] = (1, 1);
        quadrant_lookup["q8"] = (1, 0);
        quadrant_lookup["q9"] = (2, 3);
        quadrant_lookup["q10"] = (2, 2);
        quadrant_lookup["q11"] = (2, 1);
        quadrant_lookup["q12"] = (2, 0);

        foreach (KeyValuePair<string, (int, int)> entry in quadrant_lookup) 
        {

 
            
        }
            
    }

    void Update()
    {
        // global game update logic goes here
    }

    void OnGui()
    {
        // common GUI code goes here
    }
}
