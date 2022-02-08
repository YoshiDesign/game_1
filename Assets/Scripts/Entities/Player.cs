using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Custom;

public class Player : MonoBehaviour
{

    // Weapon constants
    public const int UNASSIGNED = 100;
    public const int LASER = 101;
    public const int HOMING = 102;

    /**
     * Composition stuff
     */
    [SerializeField]
    private GameObject _homing_missles;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _reticle;
    Reticles reticle;
    PlayerInputActions playerInputActions;

    /**
     * Weapons
     */

    //bool all_homing_cooldowns_complete = true;
    HomingMissle[] allHomingMissles_0;
    HomingMissle[] allHomingMissles_1;
    HomingMissle[] allHomingMissles_2;
    HomingMissle[] activeHomingMissles;
    int max_missles = 8;
    public int weapon_level_homingMissles = 1;

    /**
     * Player state
     */
    private int lives = 3;
    private int basic_weapon;
    public int special_weapon;
    public Vector3 thrust = new Vector3(0,0,500f);
    public float thrust_rate = 15.0f;

    // Cooldowns
    private float _laserCD  = 0.0f;
    public float _laserCD_time  = 0.5f;
    public float _homingCD_time = 3f;

    /**
     * Boundaries
     */
    private float upperBound = 1000.0f;
    private float lowerBound = 0.0f;
    private float rightBound = 2000.0f;
    private float leftBound  = -2000.0f;

    /**
     * Velocity
     */
    public float speedX = 500.0f;
    public float speedY = 500.0f;
    public float tilt_speed = 25.0f;
    public Vector2 dir;
    public Vector3 current_velocity;
    public Vector3 current_rotation;
    public Vector2 momentum;
    public float max_momentum = 15.0f;
    public float max_rotation = 15.0f;
    private float max_X_momentum = 55.0f;
    private float max_Y_momentum = 55.0f;

    /**
     * Reticle
     */
    public float raycast_dist = 5500.0f;
    // public Vector3 reticle_vector_1;
    public Vector3 reticle_vector_2;
    public Vector3 reticle_vector_3;

    // Other
    private AudioSource shoot_laser_sound;
    private Gamepad gamepad;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += shootBasic;
        playerInputActions.Player.ShootSpecial.performed += shootSpecial;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    void Start()
    {
        /**
         * TODO
         * Combine with homing missle Queue instantiation in Player class
         */
        allHomingMissles_0 = new HomingMissle[Helpers.HOMING_L0];
        allHomingMissles_1 = new HomingMissle[Helpers.HOMING_L1];
        allHomingMissles_2 = new HomingMissle[Helpers.HOMING_L2];

        for (int i = 0; i < max_missles; i++)
        {
            if (i < Helpers.HOMING_L0) { 
                allHomingMissles_0[i] = _homing_missles.transform.GetChild(i).gameObject.GetComponent<HomingMissle>();
            }
            if (i < Helpers.HOMING_L1) { 
                allHomingMissles_1[i] = _homing_missles.transform.GetChild(i).gameObject.GetComponent<HomingMissle>();
            }
            if (i < Helpers.HOMING_L2) { 
                allHomingMissles_2[i] = _homing_missles.transform.GetChild(i).gameObject.GetComponent<HomingMissle>();
            }
        }

        dir = new Vector2(0f, 0f);
        basic_weapon   = LASER;
        special_weapon = UNASSIGNED;

        transform.position = new Vector3(0, 50, 0);
        shoot_laser_sound = transform.GetComponent<AudioSource>();
        reticle = _reticle.GetComponent<Reticles>();

        gamepad = null;
        if (Gamepad.current != null) {
            Debug.Log("Gamepad Detected");
            gamepad = Gamepad.current;
        }

        activeHomingMissles = allHomingMissles_0;

    }

    private void Update()
    {
        dir = playerInputActions.Player.Movement.ReadValue<Vector2>();        
    }

    void FixedUpdate()
    {
        //reticle_vector_1 = (transform.position + (Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * (Vector3.forward * raycast_dist)));     // - (current_velocity * 240);
        reticle_vector_2 = (transform.position + (Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * (Vector3.forward * (raycast_dist / 8)))); // - (current_velocity * 27);
        reticle_vector_3 = transform.position + ((Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * (Vector3.forward * (raycast_dist))));// - (current_velocity * 12);
        CalculateMovement();

        if (lives == 0)
        {
            GameSystem gs = GameObject.Find("GameSystem").GetComponent<GameSystem>();
            gs.GameOver();
        }

    }

    public void CalculateMovement()
    {

        // Velocity augmentation based on pitch and roll
        CalculateRotation(dir.x, dir.y);
        getMomentum();
        checkBounds();

        current_velocity.y = (((momentum.y) / max_Y_momentum) * speedY);
        current_velocity.x = (((momentum.x) / max_X_momentum) * speedX);
   
        transform.localEulerAngles = new Vector3(current_rotation.x, current_rotation.y, current_rotation.z);
        transform.Translate(current_velocity * Time.deltaTime, Space.World);

    }

    void checkBounds()
    {

        // Right Boundary
        if (
            (rightBound - transform.position.x <= momentum.x && current_rotation.z > 300) ||
            (transform.position.x > rightBound && dir.x > 0)
        )
        {
            momentum.x = rightBound - transform.position.x;
        }

        // Left Boundary
        if (
            (leftBound - transform.position.x >= momentum.x && current_rotation.z < 100) ||
            (transform.position.x < leftBound && dir.x < 0)
        )
        {
            momentum.x = leftBound - transform.position.x;
        }

        // Top Boundary
        if (
            (upperBound - transform.position.y <= momentum.y && current_rotation.x > 300) ||
            (transform.position.y > upperBound && dir.y > 0)
        )
        {
            momentum.y = upperBound - transform.position.y;
        }

        // Bottom Boundary
        if (
            (lowerBound - transform.position.y >= momentum.y && current_rotation.x < 100) ||
            (transform.position.y < lowerBound && dir.y < 0)
        )
        {
            momentum.y = lowerBound - transform.position.y;
        }


    }
    /** 
     * Return an additive speed coeff given the current angle of roll.
     * The steeper your angle, the faster you move.
     * The ship won't move to the left while it's tilting to the right,
     * and the further to the right it's tilting, the faster it will move to the right
     * 
     * Value is currently always be between -15 and 15
     */
    void getMomentum()
    {

        // When steering
        if (current_rotation.z > 56)
        {
            momentum.x = 360.0f - current_rotation.z;
        }
        else { 
            momentum.x = current_rotation.z * -1; 
        }

        // When pitching
        if (current_rotation.x > 56)
        {
            momentum.y = 360.0f - current_rotation.x;
        }
        else { 
            momentum.y = current_rotation.x * -1; 
        }

    }
    // Roll
    void CalculateRotation(float _x, float _y)
    {
        // New angles about Z and Y axes
        if (_x != 0.0f) { 
            current_rotation.z = rotate_player(transform.localEulerAngles.z, -_x, "Z");
            current_rotation.y = rotate_player(transform.localEulerAngles.y, _x, "Y");
        }
        if (_y != 0.0f)
            current_rotation.x = rotate_player(transform.localEulerAngles.x, -_y, "X");
    }

    /**
     * @function rotate_player
     * @param float angle
     * @param float magnitude
     * @param string which
     * Do not change this implementation- Different controller inputs behave differently
     */
    float rotate_player(float angle, float magnitude, string which)
    {

        float abs_angle = Mathf.Abs(angle + ((magnitude) * tilt_speed * Time.deltaTime));

        if (abs_angle >= 55f && abs_angle < 200f)   // 200 and 220 are arbitrarily between 55 and 305 but far enough apart to avoid confusion when determining limits
        {
            return angle;
        }
        else if (abs_angle <= 305f && abs_angle > 220f)
        {
            return angle;
        }

        return angle + ((magnitude) * tilt_speed * Time.deltaTime);

    }

    // Thrust  up
    private void accelerate()
    {
        if (thrust.z < 5500) { thrust.z += (thrust_rate * Time.deltaTime);  }
    }

    // Thrust down
    private void decelerate() {
        if (thrust.z > 50) { thrust.z -= (thrust_rate * Time.deltaTime); }
    }

    /**
     * Weapons
     */
    // Upgrading Homing missles. SHould be renamed to setHomingMissleLevel
    public void upgradeHomingMissle(int n)
    {
        weapon_level_homingMissles = n;
        if (special_weapon == UNASSIGNED) {
            enableHomingMissle();
        }

        activeHomingMissles = new HomingMissle[n];
        /**
         * TODO: Find out if these are instantiated by ref || value
         */
        switch (n)
        {
            case Helpers.HOMING_L0:
                activeHomingMissles = allHomingMissles_0;
                break;
            case Helpers.HOMING_L1:
                activeHomingMissles = allHomingMissles_1;
                break;
            case Helpers.HOMING_L2:
                activeHomingMissles = allHomingMissles_2;
                break;
        }

        reticle.upgradeHomingMissle(n);
    }
    public void shootBasic(InputAction.CallbackContext ctx)
    {
        if (basic_weapon == LASER && _laserCD < Time.time) {
            StartCoroutine(shoot_triple_laser());
            _laserCD = Time.time + _laserCD_time;
        }
    }
    public void shootSpecial(InputAction.CallbackContext ctx)
    {
        if (special_weapon == UNASSIGNED) return;

        if (special_weapon == HOMING && reticle.locked_targets.Count > 0)
        {
            /**
             * TODO: Wonder about optimizations
             */
            for (int i = 0, missles_fired = 0; i < Helpers.getMaxHomingTargets(); i++)
            {
                if (activeHomingMissles[i]._cooldown > Time.time) {
                    continue;
                }
                else // Fire ze missile
                {

                    //all_homing_cooldowns_complete = false;
                    activeHomingMissles[i].target = reticle.locked_targets.Dequeue();
                    reticle.locked_targets.Enqueue(activeHomingMissles[i].target);
                    activeHomingMissles[i].gameObject.SetActive(true);
                    missles_fired++;

                    // Stop shooting once every target is accounted for, even if we have more missles left
                    if (missles_fired + 1 > reticle.locked_targets.Count)
                    {
                        break;
                    }

                }
            }
        }
    }
    private IEnumerator shoot_triple_laser()
    {
        for (int x = 3; x > 0; x--) {
            Instantiate(_laserPrefab, transform.position, transform.localRotation);
            shoot_laser_sound.Play();
            yield return new WaitForSeconds(0.1f);       
        }
    }

    /**
     * Special weapon function
     */
    public void enableHomingMissle()
    {
        special_weapon = HOMING;
        reticle.EnableHomingReticle();
    }

}
