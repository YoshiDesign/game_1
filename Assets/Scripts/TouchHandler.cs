using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

/**
 * Currently referenced by the Canvas Object
 */ 

public class TouchHandler : MonoBehaviour
{
    private Vector2 touchPosition;
    private Vector2 touchDelta;

    [SerializeField]
    private GameObject stick;
    [SerializeField]
    private GameObject toggle;

    private OnScreenCustomStick _toggle;
    private bool stick_is_active;
    [SerializeField]
    private float _attenuation_control = 7.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!Touchscreen.current.primaryTouch.press.isPressed) {
            touchDelta = new Vector2(0,0);
            print("no touch");
            stick.SetActive(false);
            if (stick_is_active)
                stick_is_active = false;
            return;

        } else
        {

            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            if (touchPosition.x < Screen.width / 2) {

                //Debug.Log("Touched: " + touchPosition);
                stick.SetActive(true);

                if (!stick_is_active)
                {
                    stick.transform.position = touchPosition;
                }

                touchDelta = Vector2.ClampMagnitude((touchDelta + Touchscreen.current.delta.ReadValue() / _attenuation_control), 50.0f);

                // Obtain the current event data and pass it to the mobile Joystick
                PointerEventData d = new PointerEventData(EventSystem.current);
                _toggle = toggle.GetComponent<OnScreenCustomStick>();
                _toggle._onDrag(d, touchDelta);

                stick_is_active = true;

            }
        }

        //if (touchDelta.x > 0.0f) {
        //    print("Swipe right");
        //}
        //if (touchDelta.x < 0.0f) {
        //    print("Swipe left");
        //}
        //if (touchDelta.y > 0.0f) {
        //    print("Swipe up");
        //}
        //if (touchDelta.y < 0.0f) {
        //    print("Swipe down");
        //}


        


    }
}
