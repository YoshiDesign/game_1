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
    public Vector3 distance;
    Vector3 armed_velocity = Vector3.forward;

    private Vector3 initial_velocity = new Vector3(5f, 14f, 0.0f);

    Vector3 tracking_vector = new Vector3(0, 0, Vector3.forward.z);     // Cross product driven
    Vector3 spinning_aimlessly = new Vector3(540f, 360f, 0);
    // Vector3 random_vector = new Vector3(Random.Range(0, 300), Random.Range(0, 300), Random.Range(0, 300));
    RaycastHit hit;

    [SerializeField]
    private int spin = 360;
    public GameObject explosionEffect;
    [SerializeField]
    GameObject player;
    Player _player;
    [SerializeField]
    private Transform missle;

    public float _cooldown = 0.0f;
    public float _destroy = 0.0f;
    private float cooldown_time = 15.0f;
    private float destroy_time = 14.0f;
    public int debug_id;
    private float fire_wait = 2f;                  // Each missle waits a small but random amount of time before it arms
    public Vector3 rot_amt;
    private Vector3 deceleration = new Vector3(0.01f, 0.05f, 0f);

    private float speed = 725f;
    int ro;
    int off;
    private bool tracking = false;
    bool destroyed = false;

    // num tells us which side the missle should spawn from initially and its initial V
    public int num;

    public Transform target;
    AudioSource explode_sound;

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
        print("MISSLE START");
        armed_velocity.z *= speed;
        // Random spin direction before the missle is armed
        ro = Random.Range(0, 1);
        ro = ro != 0 ? ro : -1;

        spinning_aimlessly  *= ro;
    }
    private void OnEnable()
    {
        print("MISSLE ENABLED");
        velocity = initial_velocity;
        velocity.y += Mathf.Max(_player.current_velocity.y, 0.1f);     // Dont add much initial Y if player is descending
        velocity.x *= num;

        // Arm the missle and start the cooldown
        StartCoroutine(ArmMissle());

        // bugger
        debug_id = target.GetInstanceID();
    }

    private void OnDisable()
    {
        // Reset the forward norm
        transform.forward = Vector3.forward;
        velocity = initial_velocity;
        tracking = false;
    }

    public IEnumerator ArmMissle()
    {
        Cooldown();
        transform.position = _player.transform.position;
        transform.GetChild(0).gameObject.SetActive(true);
        
        yield return new WaitForSeconds(fire_wait);

        if (target != null)
        {
            tracking = true;

            // Aim at target and fire
            missle.LookAt(target.position);
            transform.LookAt(target.position);
        }

        velocity = armed_velocity;

    }

    private void FixedUpdate()
    {
        
        if (target != null)
        {
            if (!tracking)
            {
                print("V: " + velocity.ToString());
                transform.Translate((velocity) * Time.deltaTime);
                velocity -= deceleration * Time.deltaTime;

                // The missle spins aimlessly when it's first ejected, ro is -1 || 1 to really spice things up
                missle.Rotate(spinning_aimlessly * Time.deltaTime, Space.World);
            } 
            else {
                transform.Translate(velocity * Time.deltaTime, Space.Self);
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
            // Keep spinnin'
            transform.Rotate((Random.Range(1.1f, 3.4f) * tracking_vector * spin) * Time.deltaTime * -1);

            // Lost it's target, keep moving
            transform.Translate(velocity * Time.deltaTime);
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
        distance = target.position - transform.position;

        if (distance.z < 500.0f) // Now yer fucked
        {
            transform.LookAt(target.position, transform.forward);
        }
        else { 

            distance.Normalize();

            rot_amt = Vector3.Cross(distance, transform.forward);//.magnitude;
            tracking_vector.x = rot_amt.x;
            tracking_vector.y = rot_amt.y;

            //tracking_vector.z = rot_amt.z;
            transform.Rotate(tracking_vector * spin * Time.deltaTime * -1);

        }

    }

    public void Cooldown()
    {
        _cooldown = Time.time + cooldown_time;
        _destroy = Time.time + destroy_time;
    }
}
