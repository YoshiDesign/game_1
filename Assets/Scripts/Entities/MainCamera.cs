using UnityEngine;
using UnityEngine.InputSystem;
using Custom;

public class MainCamera : MonoBehaviour
{

    public Player player;
    private Vector3 offset;
    private Vector3 camera_orbit;
    private Vector3 current_rotation;
    public Vector2 mousePos;
    private Vector2 mouseDelta = new Vector2(0,0);
    private float ctrx;
    private float ctry;

    private float look_up = -10;
    private float height = 9.0f;
    private float radius = 40.0f;
    public float easing = 0.0f;
    private int camera_mode = Helpers.FIRST_PERSON;
    private int update_mod = 0;

    void Start()
    {
        offset = new Vector3(0.0f, height, 0.0f);
        camera_orbit = Vector3.forward * radius;

        transform.Rotate(new Vector3(look_up, 0, 0));

        current_rotation = new Vector3(transform.eulerAngles.x, player.transform.eulerAngles.y, 0.0f);
        player = GameObject.Find("Player").GetComponent<Player>();

        ctrx = Screen.width / 2;
        ctry = Screen.width / 2;




    }

    void Update()
    {
        // Recollect the center of the screen every second or so
        update_mod++;
        if (update_mod % 100 == 0) {
            ctrx = Screen.width / 2;
            ctry = Screen.width / 2;
            update_mod = 1;
        }
        
        mousePos = Mouse.current.position.ReadValue();

    }

    private void LateUpdate()
    {


        if (camera_mode == Helpers.FIRST_PERSON) {

            current_rotation.x = mousePos.x - ctrx;
            current_rotation.y = mousePos.y - ctry;
            transform.localEulerAngles = current_rotation;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 20);

        }

        if (camera_mode == Helpers.THIRD_PERSON) {

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

    public void changeCameraMode(int mode)
    {
        camera_mode = mode;
    }
}
