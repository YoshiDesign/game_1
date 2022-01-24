using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 1200.0f;
    public float max_dist = 5500.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);

        if (transform.position.z > max_dist)
        {
            Destroy(gameObject);
        }
        else { ; }
    }
    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f)) { 
            //Debug.Log("Hit! Distance: " + hit.distance);
            //Debug.Log("Hit.point: " + hit.point);
            //Debug.Log("Hit.Collider: " + hit.collider);

            if (hit.transform.tag == "Enemy") {
                hit.transform.GetComponent<Enemy>().Destroyed();
            }
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Hit");
    //    if (other.tag == "Enemy")
    //    {
    //        Destroy(this.gameObject);
    //        //Destroy(other.gameObject);
    //    }
    //}

}
