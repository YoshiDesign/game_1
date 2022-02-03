using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    private GameObject _cam;
    private Camera cam;
    RaycastHit hit;
    Ray ray;
    Vector3 homing_reticle_ray;
    //private Vector3 reticle_vector_1_pos;
    private Vector3 reticle_vector_2_pos;
    private Vector3 reticle_vector_3_pos;
    private Player player;

    //private Transform laser_reticle_1;
    private Transform laser_reticle_2;
    private Transform laser_reticle_3;
    private Transform homing_reticle_default;
    private Transform homing_reticle_locked;


    // Start is called before the first frame update
    void Start()
    {
        cam = _cam.GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Player>();
        homing_reticle_ray = new Vector3(0,0,0);
        //laser_reticle_1 = transform.GetChild(0);
        laser_reticle_2 = transform.GetChild(1);
        laser_reticle_3 = transform.GetChild(2);
        homing_reticle_default = transform.GetChild(3);
        homing_reticle_locked = transform.GetChild(4);
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

    private void FixedUpdate()
    {
        if (homing_reticle_default.gameObject.activeSelf)
        {
            homing_reticle_ray = homing_reticle_default.transform.position;
            // Homing reticle follows reticle_vector_3
            homing_reticle_default.position = reticle_vector_3_pos;
            
            ray = cam.ViewportPointToRay(homing_reticle_ray);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Enemy") {
                    print("LOCKED");
                }
            }
        }
    }

    public void EnableHomingReticle()
    {
        homing_reticle_default.gameObject.SetActive(true);
    }

    public void TrackTarget()
    {
        
    }
}
