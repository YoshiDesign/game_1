using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
public class Mothership : Enemy { 

    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private Transform player;

    AudioSource shoot_sound;

    Vector3 player_position;
    Vector3 facing_position;

    public float max_dist = 5500.0f;

    [SerializeField]
    private Vector3 velocity;

    // Start is called before the first frame update
    protected override void Start()
    {

        player = GameObject.Find("Player").transform;

        player_position.x = player.position.x;
        player_position.y = player.position.y;
        player_position.z = player.position.z;

        facing_position.x = player_position.x;
        facing_position.y = player_position.y;
        facing_position.z = transform.position.z;

        velocity = new Vector3(-0.5f, Random.Range(-2f, -1f), Random.Range(-2f, -1f));
        shoot_sound = transform.GetComponent<AudioSource>();
        transform.LookAt(facing_position); 

        StartCoroutine(shootAtPlayer());
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Check if we should destroy this game object
        if (transform.position.z < -100.0f || transform.position.z > 5000.0f)
        {
            Destroy(this.gameObject);
        }

    }

    private void FixedUpdate()
    {
        transform.Translate(velocity, Space.World);
    }

    private IEnumerator shootAtPlayer()
    {
        while (true) {
            yield return new WaitForSeconds(Random.Range(3, 5));
            shoot();
            //velocity.z = Random.Range(-5f, -1f);
            velocity.x = Random.Range(-1f, 1f);
            velocity.y = Random.Range(-1f, 1f);
        }
    }

    public void Destroyed()
    {
        Destroy(this.gameObject);
    }

    void shoot()
    {
        // Update our knowledge of the player
        player_position.x = player.position.x;
        player_position.y = player.position.y;
        player_position.z = player.position.z;

        facing_position.x = player_position.x;
        facing_position.y = player_position.y;
        facing_position.z = player_position.z;

        transform.LookAt(facing_position);
        transform.rotation = Quaternion.LookRotation(player_position, Vector3.up);

        // spawn a laser in front of the enemy
        GameObject clone = Instantiate(laser, transform.position + (transform.forward * 50), transform.rotation);
        // Laser _laser = clone.transform.GetComponent<Laser>();

        shoot_sound.volume = .5f - (1.5f * (transform.position.z / max_dist));
        shoot_sound.Play();
    }
}
