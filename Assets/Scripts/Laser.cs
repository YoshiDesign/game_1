using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 1200.0f;
    public float max_dist = 5500.0f;
    public ParticleSystem _smoke;
    public GameObject explosionEffect;
    private AudioSource explode_sound;

    private void Start()
    {
        _smoke = transform.GetComponent<ParticleSystem>();
        explode_sound = transform.GetComponent<AudioSource>();

    }

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
                Destroy(hit.transform.gameObject);
                //hit.transform.GetComponent<Enemy>().Destroyed();
                Instantiate(explosionEffect, hit.transform.position, hit.transform.rotation);
                explode_sound.volume = .8f - (1.5f * (transform.position.z / max_dist));
                explode_sound.Play();
            }
        }
    }

}
