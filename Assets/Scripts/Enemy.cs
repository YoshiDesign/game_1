using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 4.0f;
    public float next_speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 50, 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (next_speed < Time.time) {
            speed = Random.Range(-5.0f, 5.0f);
            next_speed = Time.time + 3.0f;
        }
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    bool checkDamage()
    {
        return false;
    }
}
