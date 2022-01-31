using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAsteroid : MonoBehaviour
{
    public Vector3 velocity;
    private float swoopMagnitude;
    private float swoopStart;

    // Set upon instantiation, it's the random roll index it was selected from
    // which determines which asteroid mesh this is
    public int type;

    int rng_1;
    int rng_2;
    int rng_3;

    void Start()
    {
        rng_1 = Random.Range(0, 360);
        rng_3 = Random.Range(0, 360);
        rng_2 = Random.Range(0, 360);

        swoopStart = Random.Range(0, 3000);
        swoopMagnitude = Random.Range(10.0f, 20.0f);
        velocity = new Vector3(0, Random.Range(-5, -15), Random.Range(-1f, -2f));
    }

    void Update()
    {
        if (transform.position.z < -100.0f 
            || transform.position.z > 5000.0f
            || transform.position.x > 3000.0f)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y < swoopStart && velocity.y < 0.0f)
        {
            velocity.y += swoopMagnitude * Time.deltaTime;
        }

        transform.Rotate(new Vector3(rng_1, rng_2, rng_3) * Time.deltaTime);
        transform.Translate(velocity, Space.World);
    }
    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
