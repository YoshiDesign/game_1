using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{

    Vector3 velocity;
    GameObject player;
    public float init_x = 1.0f;
    public float init_y = 3.0f;
    public float init_z = 0.0f;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Debug.Log("Found Player: " + player.transform.position.ToString());
        velocity = new Vector3(init_x, init_y, init_z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
