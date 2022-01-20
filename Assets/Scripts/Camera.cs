using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float radius = 30.0f;

    private float cameraAngleLag = 89.0f;
    private Vector2 _input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        // This happens once, and its in the update method because the player doesn't instantiate in time and we only need this to happen once.
        // please find a way around this!
        if (
            Mathf.RoundToInt(transform.position.y) != Mathf.RoundToInt(player.transform.position.y)
            || Mathf.RoundToInt(transform.position.z) != Mathf.RoundToInt(player.transform.position.z)
            )
        {
            transform.position = player.transform.position + new Vector3(0, 0, -radius);
        }

        if (Mathf.RoundToInt(transform.eulerAngles.y) != Mathf.RoundToInt(player.transform.eulerAngles.y)) { 
            transform.RotateAround(player.transform.position, transform.up, _input.x * cameraAngleLag * Time.deltaTime);
        }

    }
}
