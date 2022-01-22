using UnityEngine;

public class Camera : MonoBehaviour
{
    public Player player;
    public float radius = 30.0f;
    public float height = 9.0f;

    private float cameraAngleLag = 89.0f;
    private Vector2 _input;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = player.transform.position + new Vector3(0, height, -radius);

        // TODO - You can just use the unit circle equation instead
        transform.RotateAround(player.transform.position, transform.up, (player.dir.x * player.current_rotation.y) * Time.deltaTime);
        if (transform.localEulerAngles.y < -15 || transform.localEulerAngles.y > 15) 
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.current_rotation.y, transform.localEulerAngles.z);

    }
}
