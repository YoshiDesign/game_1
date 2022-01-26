using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenCustomStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started onscreen stick");
        m_StartPos = Touchscreen.current.primaryTouch.position.ReadValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerUp(PointerEventData data)
    {
        SendValueToControl(0.0f);
    }

    public void _onDrag(PointerEventData data, Vector2 _delta) {
        Debug.Log("Event Data: " + data.ToString());


        //RectTransformUtility.Screen(transform.parent.GetComponentInParent<RectTransform>(), _position, data.pressEventCamera, out var position);

        ((RectTransform)transform).anchoredPosition = _delta;

        var newPos = new Vector2(_delta.x / movementRange, _delta.y / movementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerDown(PointerEventData data)
    {
        //SendValueToControl(0.0f);
    }

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    [SerializeField]
    private float m_MovementRange = 50.0f;

    [SerializeField]
    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    private Vector3 m_StartPos;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    private Vector2 delta;
}
