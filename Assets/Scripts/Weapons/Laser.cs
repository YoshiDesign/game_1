using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public class Laser : MonoBehaviour
{
    public float speed = 1800.0f;
    public GameObject explosionEffect;
    private AudioSource explode_sound;

    RaycastHit hit;

    private void Start()
    {
        explode_sound = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);

        if (   transform.position.z > Helpers.max_dist
            || transform.position.z < 0
            || transform.position.x < -2000.0f
            || transform.position.x > 2000.0f
            || transform.position.y < 0
            || transform.position.y > 2000.0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f)) { 
            //Debug.Log("Hit! Distance: " + hit.distance);
            //Debug.Log("Hit.point: " + hit.point);
            //Debug.Log("Hit.Collider: " + hit.collider);

            if (hit.transform.tag == "Enemy") {
                Destroy(hit.transform.gameObject);
                //hit.transform.GetComponent<Enemy>().Destroyed();
                GameObject expl_clone = Instantiate(explosionEffect, hit.transform.position, hit.transform.rotation);
                explode_sound.volume = .3f - (1.5f * (transform.position.z / Helpers.max_dist));
                explode_sound.Play();
                Destroy(expl_clone, 1.0f);
            }
        }
    }

}
