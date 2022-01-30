using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    private GameObject _cam;
    private Camera cam;
    //private Vector3 reticle_vector_1_pos;
    private Vector3 reticle_vector_2_pos;
    private Vector3 reticle_vector_3_pos;
    private Player player;

    //private Transform laser_reticle_1;
    private Transform laser_reticle_2;
    private Transform laser_reticle_3;

    // Start is called before the first frame update
    void Start()
    {
        cam = _cam.GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Player>();
        //laser_reticle_1 = transform.GetChild(0);
        laser_reticle_2 = transform.GetChild(1);
        laser_reticle_3 = transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        //reticle_vector_1_pos = cam.WorldToScreenPoint(player.reticle_vector_1);
        reticle_vector_2_pos = cam.WorldToScreenPoint(player.reticle_vector_2);
        reticle_vector_3_pos = cam.WorldToScreenPoint(player.reticle_vector_3);
        //reticle_vector_1_pos.z = 0.0f;
        reticle_vector_2_pos.z = 0.0f;
        reticle_vector_3_pos.z = 0.0f;
        reticle_vector_3_pos.y -= 20.0f;
        //laser_reticle_1.position = reticle_vector_1_pos;
        laser_reticle_2.position = reticle_vector_2_pos;
        laser_reticle_3.position = reticle_vector_3_pos;
    }

    void ChangeReticle()
    { 
        
    }
}
