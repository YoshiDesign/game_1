using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 next_position;

    const int MAX_Z = 6000;
    const int MAX_X = 6000;
    const int MIN_X = -6000;
    const int MAX_Y = 3000;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0.0f, 0.0f, Random.Range(-10f, -100f));
        next_position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -100.0f || transform.position.z > 5000.0f)
        {
            next_position.x = Random.Range(MIN_X, MAX_X);
            next_position.y = Random.Range(0, MAX_Y);
            next_position.z = Random.Range(5000, MAX_Z);
            transform.position = next_position;
        }

        transform.Translate(velocity);
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
