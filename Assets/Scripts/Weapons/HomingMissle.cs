using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public class HomingMissle : MonoBehaviour
{

    /**
     * This class is not used as a prefab
     */ 

    public Vector3 velocity;
    Vector3 armed_velocity;
    Vector3 initial_velocity;
    Vector3 rotation;
    Vector3 start_pos;
    Vector3 offset_pos;
    Vector3 tracking_vector;
    Vector3 spinning_aimlessly;
    Vector3 random_vector;
    public Vector3 direction;
    RaycastHit hit;

    [SerializeField]
    private int sensitivity = 360;

    AudioSource explode_sound;
    public GameObject explosionEffect;

    [SerializeField]
    GameObject player;
    Player _player;
    [SerializeField]
    private Transform missle;
    public float _cooldown = 0.0f;
    public float _destroy = 0.0f;
    private float cooldown_time = 10.0f;
    private float destroy_time = 8.0f;
    public int debug_id;
    public Vector3 rot_amt;
    [SerializeField]
    private float speed = 325f;
    int ro;
    int off;
    private bool tracking = false;
    bool destroyed = false;

    // num tells us which side to reset the missle to
    public int num;
    public Transform target;

    /**
     * TODO:
     * Multithread the implementation for cross-product "Seeking" behavior
     */
    void Awake()
    {
        explode_sound = transform.GetComponent<AudioSource>();
        _player = player.GetComponent<Player>();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Start()
    {
        tracking_vector     = new Vector3(0, 0, Vector3.forward.z);
        spinning_aimlessly  = new Vector3(540, 360, 0);
        start_pos           = new Vector3(num * 1.5f, 0, 0);
        armed_velocity      = new Vector3(0, 0, speed);
        initial_velocity    = new Vector3(0, 15.0f, 0.0f);
        rotation            = new Vector3(0, 0, 900.0f);
        random_vector = new Vector3(Random.Range(0, 300), Random.Range(0, 300), Random.Range(0, 300));

        // Left or right side
        off = Mathf.Abs(num) / num;

        // Random spin direction before the missle is armed
        ro = Random.Range(0, 1);
        ro = ro != 0 ? ro : -1;
    }
    private void OnEnable()
    {
        Cooldown(); 
        velocity = initial_velocity;
        transform.GetChild(0).gameObject.SetActive(true);

        // spawn at the player's position
        transform.position = _player.transform.position;

        // Arm the missle and start the cooldown
        StartCoroutine(ArmMissle());

        // bugger
        debug_id = target.GetInstanceID();
    }

    private void OnDisable()
    {
        // Reset the forward norm
        transform.forward = Vector3.forward;
        tracking = false;
    }

    public IEnumerator ArmMissle()
    {
        yield return new WaitForSeconds(2);
        missle.Rotate(new Vector3(45f, 0f, 0f));
        if (target != null)
        {
            tracking = true;

            // Aim at target and fire
            transform.LookAt(target.position);
        }

        velocity = armed_velocity;

    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.deltaTime);
        if (target != null)
        {

            if (!tracking)
            {
                // The missle spins aimlessly when it's first ejected, ro is -1 || 1 to really spice things up
                missle.Rotate(spinning_aimlessly * ro * Time.deltaTime, Space.Self);
            } 
            else {
                
                // The missle spins around the Z as it travels to its target
                missle.Rotate(new Vector3(0, 900 * Time.deltaTime, 0));
                trackTarget();

                if (Physics.Raycast(transform.position, transform.forward, out hit, 15.0f))
                {

                    if (hit.transform.tag == "Enemy")
                    {
                        Destroy(hit.transform.gameObject);
                        explode();
                        target = null;
                    }

                }

            }

        }
        else {
            debug_id = 0;
        }

        // Destroy is separate from cooldown bc SetActive doesn't have a delay like Destroy does. Plus, we're not destroying, this class isn't a prefab
        if (_destroy < Time.time && !destroyed)
        {

            // Hide the warhead
            transform.GetChild(0).gameObject.SetActive(false);
            destroyed = true;
            explode();
        }
        if (_cooldown < Time.time) { 
            this.gameObject.SetActive(false);
        }
    }

    private void explode()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject expl_clone = Instantiate(explosionEffect, transform.position, transform.rotation);
        explode_sound.volume = .3f - (1.5f * (transform.position.z / Helpers.max_dist));
        explode_sound.Play();
        this.gameObject.SetActive(false);
        Destroy(expl_clone, 1.0f);
    }

    public void trackTarget()
    {
        direction = target.position - transform.position;

        if (direction.z < 100.0f) // Now yer fucked
        {
            transform.LookAt(target.position, transform.forward);
        }
        else { 

            direction.Normalize();

            rot_amt = Vector3.Cross(direction, transform.forward);//.magnitude;
            tracking_vector.x = rot_amt.x;
            tracking_vector.y = rot_amt.y;

            //tracking_vector.z = rot_amt.z;
            transform.Rotate(tracking_vector * sensitivity * Time.deltaTime * -1);

        }

    }

    public void Cooldown()
    {
        _cooldown = Time.time + cooldown_time;
        _destroy = Time.time + destroy_time;
    }
}
