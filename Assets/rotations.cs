using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotations : MonoBehaviour
{
    [SerializeField]
    Transform otherObject;

    float degreesPerSecond = 20;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(new Vector3(0, degreesPerSecond, 0) * Time.deltaTime);
        rotateAround();
    }


    void followRotation()
    { 
    
    }

    void rotateFacing() 
    {
    
    }

    void rotateAround()
    {
        transform.RotateAround(otherObject.position, Vector3.right, 25 * Time.deltaTime);
    }
}
