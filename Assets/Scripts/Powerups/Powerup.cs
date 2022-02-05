using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float max_distance = 3900.0f;

    private Player player;

    [SerializeField]
    private Vector3 velocity = new Vector3(0, 0, -500f);

    // Start is called before the first frame update
    void Start()
    {
        velocity *= Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {

            player = other.GetComponent<Player>();
            Destroy(this.gameObject);

            // Determine which powerup to enable
            switch (this.tag) {

                case "homing_missle_powerup": 
                    if (player.weapon_level_homingMissles < 8)
                        player.upgradeHomingMissle(player.weapon_level_homingMissles * 2); // Dont ask
                    break;

                default:
                    print("Default Powerup");
                    break;
            
            }

        }
    }
}
