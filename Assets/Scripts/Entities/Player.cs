using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    const int UNASSIGNED = 0;
    const int LASER = 1;
    const int HOMING = 2;

    private int basic_weapon;
    private int special_weapon;

    [SerializeField]
    private GameObject _laserPrefab;
    PlayerInputActions playerInputActions;

    private float upperBound = 1000.0f;
    private float lowerBound = 0.0f;
    private float rightBound = 2000.0f;
    private float leftBound = -2000.0f;

    // Speed and direction
    public float speedX = 500.0f;
    public float speedY = 500.0f;
    public float tilt_speed = 25.0f;
    
    // If we hit a boundary, the momentum will be set to 0.
    // It then needs to gradually reset to what it was prior to hitting the boundary
    public float rampX = 1.0f;
    public float rampY = 1.0f;
    public float rampX_rate = 1f;
    public float rampY_rate = 1f;

    // Actual current direction independent of controller press.
    private int _dirX = 0;
    private int _dirY = 0;

    // Magnitude of deceleration when the player cannot control the craft
    // due to being out of bounds
    [SerializeField]
    private float stall = 80f;

    public Vector2 dir;
    public Vector3 current_velocity;
    public Vector3 current_rotation;

    public float raycast_dist = 5500.0f;
    private float max_X_momentum = 55.0f;
    private float max_Y_momentum = 55.0f;

    public Vector3 reticle_vector_1;
    public Vector3 reticle_vector_2;
    public Vector3 reticle_vector_3;

    private float _canShoot = 0.0f;

    // Momentum- *Z-angle (roll) alters speed
    public float max_momentum = 15.0f;
    public float max_rotation = 15.0f;
    public Vector2 momentum;

    private int lives = 3;
    private int current_weapon = 1;

    private AudioSource shoot_laser_sound;
    private Gamepad gamepad;

    // RaycastHit reticle;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += shootLaser;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    void Start()
    {
        dir = new Vector2(0f, 0f);
        basic_weapon = LASER;
        special_weapon = HOMING;

        transform.position = new Vector3(0, 50, 0);
        shoot_laser_sound = transform.GetComponent<AudioSource>();

        gamepad = null;
        if (Gamepad.current != null) { 

            gamepad = Gamepad.current;
            Debug.Log("Gamepad Detected");
        }
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

        current_velocity.y = (((momentum.y) / max_Y_momentum) * speedY) * Time.deltaTime;
        current_velocity.x = (((momentum.x) / max_X_momentum) * speedX) * Time.deltaTime;
   
        transform.localEulerAngles = new Vector3(current_rotation.x, current_rotation.y, current_rotation.z);
        transform.Translate(current_velocity, Space.World);

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

    // Bang
    public void shootLaser(InputAction.CallbackContext ctx)
    {
        if (_canShoot < Time.time) { 
        
            if (current_weapon == 1) {
                StartCoroutine(shoot_laser());
                _canShoot = Time.time + 0.5f;
            }

        }
    }

    private IEnumerator shoot_laser()
    {
        for (int x = 3; x > 0; x--) {
            Instantiate(_laserPrefab, transform.position, transform.localRotation);
            shoot_laser_sound.Play();
            yield return new WaitForSeconds(0.1f);       
        }
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

        if (abs_angle >= 55f && abs_angle < 200f)   // 200 and 220 are arbitrarily between 55 adn 305 but far enough apart to avoid confusion when determining limits
        {
            return angle;
        }
        else if (abs_angle <= 305f && abs_angle > 220f) {
            return angle;
        }

        return angle + ((magnitude) * tilt_speed * Time.deltaTime);

    }

}
