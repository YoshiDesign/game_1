using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0f, 0f, Random.Range(1f, -600f));
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

        if (transform.position.z < -100.0f) {
            GameObject.Destroy(this.gameObject);
        }

    }
    private bool checkDamage()
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        if (other.tag == "Laser" || other.tag == "Player") {
            Destroy(this.gameObject);
        }
    }
}
