using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0f, 0f, Random.Range(-1f, -10f));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDestroy();
    }

    private void Move()
    {
        transform.Translate(velocity);
    }
    private void CheckDestroy() 
    {

        if (transform.position.z < -100.0f || transform.position.z > 5000.0f) {
            Destroy(this.gameObject);
        }

    }
    public void Destroyed()
    {
        Destroy(this.gameObject);
    }
}
