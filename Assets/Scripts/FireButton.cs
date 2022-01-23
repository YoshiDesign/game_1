using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour
{
    public OnScreenButton button;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("FireButton");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Pressed");
    }
}
