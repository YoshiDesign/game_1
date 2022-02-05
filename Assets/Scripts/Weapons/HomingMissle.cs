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
    Vector3 rotation;
    Vector3 start_pos;
    Vector3 offset_pos;
    Vector3 track;
    public Vector3 direction;
    RaycastHit hit;

    AudioSource explode_sound;
    public GameObject explosionEffect;

    [SerializeField]
    GameObject player;
    Player _player;
    [SerializeField]
    private Transform missle;

    private float init_x = 2.0f;
    private float init_y;
    private float init_z;
    public float _cooldown = 0.0f;
    public float _destroy = 0.0f;
    private float cooldown_time = 10.0f;
    private float destroy_time = 8.0f;
    public int debug_id;
    public Vector3 rot_amt;
    private float speed = 525f;
    int ro;
    int off;
    private bool tracking = false;

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
    }

    void Start()
    {

        init_y = Random.Range(1, 10);
        init_z = Random.Range(-1, 5);

        start_pos = new Vector3(num * 1.5f, 0, 0);
        armed_velocity = new Vector3(0, 0, speed);
        rotation  = new Vector3(0, 0, 900.0f);

        // Left or right side
        off = Mathf.Abs(num) / num;

        // Random spin direction before the missle is armed
        ro = Random.Range(0, 1);
        ro = ro != 0 ? ro : -1;
    }
    private void OnEnable()
    {
        // spawn at the player's position
        transform.position = _player.transform.position;

        velocity = _player.current_velocity;
        // Arm the missle and start the cooldown
        StartCoroutine(ArmMissle());
        Cooldown();
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
        missle.Rotate(new Vector3(45f, 0f, 0f));

        yield return new WaitForSeconds(2);

        tracking = true;

        // Aim at target and fire
        transform.LookAt(target.position);
        velocity = armed_velocity;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {

            if (!tracking)
            {
                // Starting rotation is sync'd with the arm timer   
                missle.Rotate(new Vector3(540, 360, 0) * ro * Time.deltaTime, Space.Self);
            } 
            else {
                missle.Rotate(new Vector3(0, 0, 900 * Time.deltaTime));
                //transform.Rotate(Vector3.forward, 100f * Time.deltaTime, Space.Self
                trackTarget();

                if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f))
                {

                    if (hit.transform.tag == "Enemy")
                    {
                        Destroy(hit.transform.gameObject);
                        //hit.transform.GetComponent<Enemy>().Destroyed();
                        explode();
                        target = null;
                    }
                }

            }

        }
        else {
            debug_id = 0;
        }

        // Reposition in the ship and deactivate.
        if (_destroy < Time.time)
        {
            explode();
        }
        if (_cooldown < Time.time) { 
            this.gameObject.SetActive(false);
        }
    }

    private void explode()
    {
        GameObject expl_clone = Instantiate(explosionEffect, hit.transform.position, hit.transform.rotation);
        explode_sound.volume = .3f - (1.5f * (transform.position.z / Helpers.max_dist));
        explode_sound.Play();
        Destroy(expl_clone, 1.0f);
    }



    public void trackTarget()
    {
        direction = target.position - transform.position;
        direction.Normalize();

        rot_amt = Vector3.Cross(direction, transform.forward);//.magnitude;

        transform.Rotate(new Vector3(rot_amt.x * 360, rot_amt.y * 360, rot_amt.z * 360) * Time.deltaTime * -1);
        transform.Translate(velocity * Time.deltaTime);
    }

    public void Cooldown()
    {
        _cooldown = Time.time + cooldown_time;
        _destroy = Time.time + destroy_time;
    }
}
