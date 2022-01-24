using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class TouchHandler : MonoBehaviour
{
    public Vector2 touchPosition;
    public Vector2 touchDelta;

    public GameObject stick;
    public GameObject toggle;
    private OnScreenCustomStick _toggle;
    private bool stick_is_active;

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

                PointerEventData d = new PointerEventData(EventSystem.current);
                _toggle = toggle.GetComponent<OnScreenCustomStick>();
                _toggle._onDrag(d, touchPosition,Touchscreen.current.delta.ReadValue());

                stick_is_active = true;

            }
        }

        touchDelta = Touchscreen.current.delta.ReadValue();

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
