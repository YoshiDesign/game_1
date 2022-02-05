using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    [SerializeField]
    private GameObject mothership;

    [SerializeField]
    private float max_spawn_distance = 3000.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(SpawnMothership());
    }


    public IEnumerator SpawnMothership()
    {
        while (true) { 
            Instantiate(mothership, new Vector3(Random.Range(-1400, 2000), Random.Range(50, 1000), max_spawn_distance), Quaternion.Inverse(Quaternion.identity));
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
