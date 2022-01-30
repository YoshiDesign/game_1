using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "asteroid")
            velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, -8), Random.Range(-1f, -20f));
        else
            velocity = new Vector3(0.0f, 0.0f, Random.Range(-10f, -100f));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -100.0f || transform.position.z > 5000.0f)
        {
            Destroy(this.gameObject);
        }

        transform.Translate(velocity);
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
