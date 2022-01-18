using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float currentRotationZ = 0.0f;
    float currentRotationY = 0.0f;
    float currentRotationX = 0.0f;
    Quaternion newZY;
    Quaternion newX;


    public float speed = 290.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    public float fire_rate = 0.5f;
    public float can_fire = -1.0f;

    public float tilt_speed = 113.0f;
    public float pitch_speed = 5.0f;
    public Vector2 _input;

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

    void CalculateMovement()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        // There is motion
        if (_input.x != 0.0f || _input.y != 0.0f)
        {

            if ((_input.x < 1.0f || _input.x > -1.0f))
                CalculateADMovement(_input.x, _input.y);
            if ((_input.y > -1.0f || _input.y < 1.0f))
                CalculateWSMovement(_input.x, _input.y);

            transform.rotation = Quaternion.Euler(new Vector3(currentRotationX, currentRotationY, currentRotationZ));
            transform.Translate(new Vector3(_input.x, _input.y, 0) * speed * Time.deltaTime, Space.World);
        }

    }

    // Roll
    void CalculateADMovement(float _x, float _y)
    {
        // New angles about Z and Y axes
        currentRotationZ = rotate_player(transform.localEulerAngles.z, -_x, "Z");
        currentRotationY = rotate_player(transform.localEulerAngles.y, _x, "Y");
    }

    // Pitch
    void CalculateWSMovement(float _x, float _y)
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
