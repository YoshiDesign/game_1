using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 1000.0f;
    public float max_dist = 500.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, speed * Time.deltaTime);

        if (transform.position.z > max_dist) {
            Destroy(gameObject);
        }
    }
}
