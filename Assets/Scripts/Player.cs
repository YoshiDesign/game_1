using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;

    private float currentRotationZ = 0.0f;
    private float currentRotationY = 0.0f;
    private float currentRotationX = 0.0f;

    float attenuateFromDegree = 0.0f;

    public float speedX = 125.0f;
    public float speedY = 110.0f;

    // Maximum roll the player can achieve. Effects speed
    public float max_momentum = 15.0f;
    float momentumX = 0.0f;
    float momentumY = 0.0f;
    float drift = 0.0f;     // Rate of roll speed increase
    public float fire_rate = 0.5f;
    public float can_fire = -1.0f;
    public float tilt_speed = 25.0f;
    public float pitch_speed = 5.0f;
    public bool lock_velocity = false;
    public Vector2 dir;
    private Vector3 current_velocity;
    private Vector3 current_rotation;

    void Start()
    {
        transform.position = new Vector3(0, 50, 0);
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > can_fire)
        { 
            shootLaser();
        }
            
    }
    // Return a speed variable given the current angle of roll.
    float getMomentum(string axis)
    {
        if (axis == "X") {
            // When steering right
            if (currentRotationZ > 16)
            {
                //Debug.Log("Current RotationZ: " + currentRotationZ);
                return 360.0f - currentRotationZ;
            }

            // When steering left
            return currentRotationZ * -1;
        }

        if (axis == "Y") {
            // When steering right
            if (currentRotationX > 16)
            {
                //Debug.Log("Current RotationX: " + currentRotationX);
                return 360.0f - currentRotationX;
            }

            // When steering left
            return currentRotationX * -1;
        }

        return 0.0f;

    }
    void CalculateMovement()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

        momentumX = getMomentum("X");
        momentumY = getMomentum("Y");

        if (dir.x != 0.0f)
        {
            CalculateADRotation(dir.x, dir.y);

            // Don't roll until momentum and direction are pointing in the same direction
            if ((dir.x < 0.0f && momentumX < 0.0f) || (dir.x > 0.0f && momentumX > 0.0f))
                current_velocity.x = dir.x * speedX  * Time.deltaTime;

        }
        else if (momentumX != 0.0f) {
            
            current_velocity.x = (momentumX / Mathf.Abs(momentumX)) * ((speedX - max_momentum) + momentumX) * Time.deltaTime;
        }
        if (dir.y != 0.0f)
        {
            CalculateWSRotation(dir.x, dir.y);
            // Don't pitch until momentum and direction are pointing in the same direction
            if ((dir.y < 0.0f && momentumY < 0.0f) || (dir.y > 0.0f && momentumY > 0.0f))
                current_velocity.y = dir.y * speedY * Time.deltaTime;
        }
        else if (momentumY != 0.0f) {
            current_velocity.y = (momentumY / Mathf.Abs(momentumY)) * ((speedY - max_momentum) + momentumY) * Time.deltaTime;
        }

        current_rotation.x = currentRotationX;
        current_rotation.y = currentRotationY;
        current_rotation.z = currentRotationZ;

        Debug.Log("D.X : " + dir.x);
        Debug.Log("D.Y : " + dir.y);
        Debug.Log("momY : " + momentumY);
        Debug.Log("momX : " + momentumX);

        transform.rotation = Quaternion.Euler(current_rotation);
        transform.Translate(current_velocity, Space.World);

    }

    // Roll
    void CalculateADRotation(float _x, float _y)
    {
        // New angles about Z and Y axes
        currentRotationZ = rotate_player(transform.localEulerAngles.z, -_x, "Z");
        currentRotationY = rotate_player(transform.localEulerAngles.y, _x, "Y");
    }

    // Pitch
    void CalculateWSRotation(float _x, float _y)
    {
        // New angle about X axis
        currentRotationX = rotate_player(transform.localEulerAngles.x, -_y, "X");
    }

    void shootLaser()
    {
        // Reset cooldown
        can_fire = Time.time + fire_rate;
        Instantiate(_laserPrefab, transform.position, transform.localRotation);
       
    }

    float rotate_player(float angle, float magnitude, string which)
    {
        
        angle = angle + ((magnitude) * tilt_speed * Time.deltaTime);

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

        //Debug.Log(which + " : " + angle + " : " + magnitude);

        return angle;
    }

}
