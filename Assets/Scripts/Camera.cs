using UnityEngine;

public class Camera : MonoBehaviour
{
    public Player player;
    public float radius = -30.0f;
    public float height = 9.0f;
    public Vector3 offset;

    private float cameraAngleLag = 89.0f;
    private Vector2 _input;
    
    void Start()
    {
        offset = new Vector3(0f, height, radius);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    
    void Update()
    {
        // Keep the camera behind the player

        // Remember that the player's X momentum should govern the camera's (and the player's, while we're here) Y rotation
        transform.rotation = Quaternion.AngleAxis(player.momentum.x, Vector3.up);
        transform.position = player.transform.TransformPoint(Vector3.forward * -5) + offset;
        
    }
}
