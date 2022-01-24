using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Player _player;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponent<Text>();
        _player = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 c_velocity = _player.current_velocity;
        Vector3 c_rotation = _player.current_rotation;
        text.text = "Velocity: " + c_velocity.ToString();
        text.text += "\nRotation: " + c_rotation.ToString();
        text.text += "\nPos: " + _player.transform.position.ToString();
    }
}
