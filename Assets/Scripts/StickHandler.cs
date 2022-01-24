using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StickHandler : MonoBehaviour
{
    private Vector2 touchPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!Touchscreen.current.primaryTouch.press.isPressed) {
            return;
        }

        touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        //Touchscreen.current.primaryTouch
        Vector2 touchDelta = Touchscreen.current.delta.ReadValue();

        //Debug.Log("Touched: " + touchPosition);
        //Debug.Log("Touch Delta: " + touchDelta);

    }
}
