using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float max_spawn_distance = 3000.0f;

    private Queue<GameObject> enemies = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {

        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            for (int j = 0; j < Random.Range(10, 20); j++)
            {
                enemies.Enqueue(Instantiate(
                    enemy,
                    new Vector3(
                        (float)i * Random.Range(-150, 150),
                        (float)j * Random.Range(30, 70),
                        max_spawn_distance + Random.Range(0, 1000)),
                    Quaternion.identity));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
