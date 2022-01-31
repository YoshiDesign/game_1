using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Player player;
    private Vector3 offset;
    private Vector3 camera_orbit;
    private Vector3 current_rotation;
    private float look_up = -10;
    private float height = 9.0f;
    private float radius = -40.0f;

    void Start()
    {
        offset = new Vector3(0.0f, height, 0.0f);
        camera_orbit = Vector3.forward * radius;
        transform.Rotate(new Vector3(look_up, 0, 0));

        current_rotation = new Vector3(transform.eulerAngles.x, player.transform.eulerAngles.y, 0.0f);

        player = GameObject.Find("Player").GetComponent<Player>();

        // Tilt the camera up. This sets our view and the X rotation doesn't change
    }

    void Update()
    {

    }
    private void FixedUpdate()
    {

        // Remember that the player's X momentum should govern the camera's (and the player's, while we're here) Y rotation

        current_rotation.y = player.transform.eulerAngles.y;
        transform.eulerAngles = current_rotation;

        // Tight Camera
        transform.position = (player.transform.position + Quaternion.Euler(0.0f, (player.momentum.x), 0.0f) * camera_orbit) + offset;

        // Loose Camera
        //transform.position = player.transform.position + camera_orbit + offset;

    }
}
