using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Quaternion currentRotation;
    private Vector3 currentRotationAngles;

    [SerializeField]
    private float speed = 13.5f;
    [SerializeField]
    private GameObject _laserPrefab;

    public float tilt_speed = 113.0f;
    public float pitch_speed = 5.0f;
    public float horizontalInput = 0.0f;
    public float verticalInput = 0.0f;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Debug.Log("H: " + horizontalInput);
        //Debug.Log("V: " + verticalInput);

        if (horizontalInput > 0.0f || horizontalInput < 0.0f) CalculateADMovement(horizontalInput, verticalInput);
        if (verticalInput > 0.0f || verticalInput < 0.0f) CalculateWSMovement(horizontalInput, verticalInput);
    }

    void CalculateADMovement(float horizontalInput, float verticalInput)
    {
        currentRotationAngles = transform.rotation.eulerAngles;
        currentRotationAngles.z += (-horizontalInput * tilt_speed * Time.deltaTime);

        Debug.Log("Current R-Z: " + currentRotationAngles.z);

        if (currentRotationAngles.z > 15
            && currentRotationAngles.z < 355
            && horizontalInput < 0) currentRotationAngles.z = 15;
        if (currentRotationAngles.z < 355 
            && currentRotationAngles.z > 15
            && horizontalInput > 0) currentRotationAngles.z = 345;

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(currentRotationAngles.z, Vector3.forward);

        // Movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 3.8f), 0);
        
    }
    
    void CalculateWSMovement(float horizontalInput, float verticalInput)
    {
        currentRotation = transform.rotation;
        currentRotationAngles = currentRotation.eulerAngles;

        // Rotate about the X axis
        currentRotationAngles += new Vector3(pitch_speed * Time.deltaTime, 0, currentRotationAngles.z);
        currentRotation.eulerAngles = currentRotationAngles;
        // Movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 3.8f), 0);


    }
}
