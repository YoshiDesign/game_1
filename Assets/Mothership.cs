using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour
{
    [SerializeField]
    private GameObject laser;

    AudioSource shoot_sound;

    private Player player;

    public float max_dist = 5500.0f;


    private float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        shoot_sound = transform.GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        transform.LookAt(player.transform);
        transform.Rotate(0, 180, 0, Space.Self);
        Debug.Log(transform.forward);
        StartCoroutine(shootAtPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    private IEnumerator shootAtPlayer()
    {
        while (true) {
            speed = Random.Range(1f, 3f);
            Debug.Log(speed * Time.deltaTime);
            yield return new WaitForSeconds(Random.Range(3, 8));
            shoot();
        }
    }

    void shoot()
    {
        print("BANG!");
        transform.LookAt(player.transform, Vector3.up);
        transform.Rotate(0, 180, 0, Space.Self);
        GameObject clone = Instantiate(laser, transform.position + (transform.forward * -50), Quaternion.identity);
        Laser _laser = clone.transform.GetComponent<Laser>();
        _laser.speed *= -1;

        shoot_sound.volume = .8f - (1.5f * (transform.position.z / max_dist));
        shoot_sound.Play();
    }
}
