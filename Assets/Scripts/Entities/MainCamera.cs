using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Player player;
    private Vector3 offset;
    private Vector3 camera_orbit;
    private Vector3 current_rotation;
    private float look_up = -10;
    private float height = 9.0f;
    private float radius = 40.0f;
    public float easing = 0.0f;

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

        // Loose Camera
        //transform.position = player.transform.position - camera_orbit + offset;

        // Tight Camera
        transform.position = new Vector3(
            player.transform.position.x - (Mathf.Cos((player.current_rotation.y + 270) * 0.017453f) * radius),
            player.transform.position.y + height,
            player.transform.position.z - (Mathf.Sin((player.current_rotation.y + 270) * 0.017453f) * radius * -1)
        );

        current_rotation.y = player.transform.eulerAngles.y;
        transform.eulerAngles = current_rotation;

    }
}
