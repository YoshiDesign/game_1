using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject touchHandler ;
    [SerializeField]
    private GameObject gameSystem;

    private GameSystem _sys;
    private TouchHandler _touch;
    private Player _player;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Debug ???");
        _touch = touchHandler.GetComponent<TouchHandler>();
        text = transform.GetComponent<Text>();
        _player = player.GetComponent<Player>();
        _sys = gameSystem.GetComponent<GameSystem>();


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 c_velocity = _player.current_velocity;
        Vector3 c_rotation = _player.current_rotation;

        text.text = "Direction: "    + _player.dir.ToString();
        text.text += "\nVelocity: "  + c_velocity.ToString();
        text.text += "\nRotation: "  + c_rotation.ToString();
        text.text += "\nPos: "       + _player.transform.position.ToString();
        text.text += "\n\nTouch Debug: ";
        text.text += "\nActive: "    + _touch.touchPosition.ToString();
        text.text += "\nDelta: "     + _touch.touchDelta.ToString();
        text.text += "\n\nObjects: " + _sys.no_enemies;
    }
}
