using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    PlayerInputActions playerInputActions;

    public float speedX = 125.0f;
    public float speedY = 110.0f;
    public Vector2 tmp_dir;
    public Vector2 dir;
    public Vector3 current_velocity;
    public Vector3 current_rotation;

    // Maximum roll the player can achieve. Effects speed
    public float max_momentum = 15.0f;
    public float max_rotation = 15.0f;
    public Vector2 momentum;
    private int lives = 3;

    float drift = 0.0f;     // Rate of roll speed increase [UNUSED]
    public float tilt_speed = 25.0f;
    public float pitch_speed = 5.0f;
    public bool lock_velocity = false;

    private AudioSource _laser_sound;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += shootLaser;
    }

    

    void Start()
    {
        dir = new Vector2(0f, 0f);
        transform.position = new Vector3(0, 50, 0);
        _laser_sound = transform.GetComponent<AudioSource>();
    }

    private void Update()
    {
        tmp_dir = playerInputActions.Player.Movement.ReadValue<Vector2>();

        // Because Touchscreen input delta can be > 1 but gamepad and keyboard can't be > 1 ..wtf
        if (tmp_dir.x != 0.0f)
        {
            dir.x = Mathf.Abs(tmp_dir.x) / tmp_dir.x;
        }
        else dir.x = 0.0f;

        if (tmp_dir.y != 0.0f)
        {
            dir.y = Mathf.Abs(tmp_dir.y) / tmp_dir.y;
        }
        else dir.y = 0.0f;

    }

    void FixedUpdate()
    {
        
        CalculateMovement();

        if (lives == 0)
        {
            GameSystem gs = GameObject.Find("GameSystem").GetComponent<GameSystem>();
            gs.GameOver();
        }

    }

    //public void MovePlayer(InputAction.CallbackContext context)
    //{
    //    dir = context.ReadValue<Vector2>();
    //    Debug.Log("At position: (" + dir.x + ", " + dir.y + ")");

    //}

    public void CalculateMovement()
    {
        
        // Velocity augmentation based on pitch and roll
        getMomentum();
        // 
        CalculateADRotation(dir.x, dir.y);
        CalculateWSRotation(dir.x, dir.y);

        // Continue moving X based on our momentum given by angle of rotation
        current_velocity.x = ((speedX / max_momentum) * momentum.x) * Time.deltaTime;

        // Continue moving Y based on our momentum given by angle of pitch
        current_velocity.y = ((speedY / max_momentum) * momentum.y) * Time.deltaTime;

        transform.localEulerAngles = new Vector3(current_rotation.x, current_rotation.y, current_rotation.z);
        transform.Translate(current_velocity, Space.World);

    }
    /* 
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
        if (current_rotation.z > 16) momentum.x = 360.0f - current_rotation.z;
        else momentum.x = current_rotation.z * -1;

        // When pitching
        if (current_rotation.x > 16) momentum.y = 360.0f - current_rotation.x;
        else momentum.y = current_rotation.x * -1;

    }
    // Roll
    void CalculateADRotation(float _x, float _y)
    {
        // New angles about Z and Y axes
        current_rotation.z = rotate_player(transform.localEulerAngles.z, -_x, "Z");
        current_rotation.y = rotate_player(transform.localEulerAngles.y, _x, "Y");
    }

    // Pitch
    void CalculateWSRotation(float _x, float _y)
    {
        // New angle about X axis
        current_rotation.x = rotate_player(transform.localEulerAngles.x, -_y, "X");
    }
    // Bang
    public void shootLaser(InputAction.CallbackContext ctx)
    {
        Instantiate(_laserPrefab, transform.position, transform.localRotation);
        _laser_sound.Play();
    }

    float rotate_player(float angle, float magnitude, string which)
    {
        
        angle = angle + ((magnitude) * tilt_speed * Time.deltaTime);

        // Lock the maximum rotation
        if (angle > 15 && angle < 345)
        {
            // TODO condense theses when optimizing. This just helps me debug
            if      (which == "Y" && magnitude > 0) angle = 15;
            else if (which == "Z" && magnitude > 0) angle = 15;
            else if (which == "X" && magnitude > 0) angle = 15;

            if      (which == "Y" && magnitude < 0) angle = 345;
            else if (which == "Z" && magnitude < 0) angle = 345;
            else if (which == "X" && magnitude < 0) angle = 345;

        }

        return angle;
    }

}
