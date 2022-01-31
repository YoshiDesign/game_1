using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    PlayerInputActions playerInputActions;

    // Speed and direction
    public float speedX = 125.0f;
    public float speedY = 110.0f;
    public float tilt_speed = 25.0f;
    public float pitch_speed = 5.0f;
    public float vertical_stall_rate = 5.0f;
    private bool stalling = false;

    public Vector2 tmp_dir;
    public Vector2 dir;
    public Vector3 current_velocity;
    public Vector3 current_rotation;
    public float raycast_dist = 5500.0f;

    public Vector3 reticle_vector_1;
    public Vector3 reticle_vector_2;
    public Vector3 reticle_vector_3;

    // Momentum- *Z-angle (roll) alters speed
    public float max_momentum = 15.0f;
    public float max_rotation = 15.0f;
    public Vector2 momentum;

    private int lives = 3;
    private int current_weapon = 1;

    private AudioSource shoot_laser_sound;
    private Gamepad gamepad;

    RaycastHit reticle;

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

    /**
     * 
     * 
     * 
     * 
     * 
     * TODO
     * 
     * Check for gamepad
     * If gamepad, attenuate rotation?
     * 
     * 
     * 
     * 
     */

    void Start()
    {
        dir = new Vector2(0f, 0f);
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
        getMomentum();
        CalculateRotation(dir.x, dir.y);

        // Continue moving X based on our momentum given by angle of rotation
        current_velocity.x = ((speedX / max_momentum) * momentum.x) * Time.deltaTime;

        // Continue moving Y based on our momentum given by angle of pitch
        if (stalling || transform.position.y >= 1300.0f)
        {
            Debug.Log("Stalling");
            stalling = true;
            current_velocity.y = momentum.y < 0 ? momentum.y : current_velocity.y - (vertical_stall_rate * Time.deltaTime);
        }
        else if (!stalling)
        {
            Debug.Log("Not Stalling");
            current_velocity.y = ((speedY / max_momentum) * momentum.y) * Time.deltaTime;
        }
        else if (transform.position.y <= 1290.0f) {
            Debug.Log("Stalling Reset");
            stalling = false; 
        }

        transform.localEulerAngles = new Vector3(current_rotation.x, current_rotation.y, current_rotation.z);
        transform.Translate(current_velocity, Space.World);

    }
    /** `
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
        if (current_weapon == 1) {
            StartCoroutine(shoot_lazer());
        }
    }

    private IEnumerator shoot_lazer()
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
