using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Quaternion currentRotation;
    private Vector3 currentRotationAngles;
    float currentRotationZ;

    [SerializeField]
    private float speed = 90.0f;
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

        //Debug.Log("H: " + horizontalInput);
        //Debug.Log("V: " + verticalInput);

        if (_input.x > 0.0f || _input.x < 0.0f) CalculateADMovement(_input.x, _input.y);
        if (_input.y > 0.0f || _input.y < 0.0f) CalculateWSMovement(_input.x, _input.y);
    }

    void CalculateADMovement(float _x, float _y)
    {
        currentRotationZ = transform.localEulerAngles.z + (-_x * tilt_speed * Time.deltaTime);

        if (currentRotationZ > 15
            && currentRotationZ < 345
            && _x < 0) currentRotationZ = 15;
        if (currentRotationZ < 345 
            && currentRotationZ > 15
            && _x > 0) currentRotationZ = 345;

        transform.Translate(new Vector3(_x, 0, 0) * speed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.AngleAxis(currentRotationZ, Vector3.forward);

        // Movement
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
    }
    
    void CalculateWSMovement(float _x, float _y)
    {
        currentRotation = transform.rotation;
        currentRotationAngles = currentRotation.eulerAngles;

        // Rotate about the X axis
        currentRotationAngles += new Vector3(pitch_speed * Time.deltaTime, 0, currentRotationAngles.z);
        currentRotation.eulerAngles = currentRotationAngles;
        // Movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 1.0f, 50.0f), 0);


    }

    void shootLaser()
    {

        can_fire = Time.time + fire_rate;
        Instantiate(_laserPrefab, transform.position, Quaternion.identity);
       
    }

}
