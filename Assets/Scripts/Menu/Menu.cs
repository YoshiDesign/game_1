using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    private PlayerInputActions playerInputActions;
    public GameObject PlayButton;
    public GameObject OptionsButton;
    public GameObject PlayButtonHovered;
    public GameObject OptionsButtonHovered;
    public EventSystem eventSys;

    private void Start()
    {
        
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public void Hovered(Event e)
    {
        Debug.Log("EVENT");
        
    }

    // Update is called once per frame
    void Update()
    {

        if (eventSys.currentSelectedGameObject != null) {
            Debug.Log(eventSys.currentSelectedGameObject.transform.tag);
        }

        if (playerInputActions.UI.Click.triggered) {
            
        }
    }
}
