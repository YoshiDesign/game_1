using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject reticle;
    private Reticles _reticle;
    //[SerializeField]
    //private GameObject touchHandler ;
    [SerializeField]
    private GameObject gameSystem;

    private GameSystem _sys;
    //private TouchHandler _touch;
    private Player _player;
    private Text text;

    [SerializeField]
    private GameObject camera;
    private Camera _camera;
    private MainCamera _main;

    // Start is called before the first frame update
    void Start()
    {
        //_touch = touchHandler.GetComponent<TouchHandler>();

        text = transform.GetComponent<Text>();
        _reticle = reticle.GetComponent<Reticles>();
        _player = player.GetComponent<Player>();
        _sys = gameSystem.GetComponent<GameSystem>();
        _camera = camera.GetComponent<Camera>();
        _main = camera.GetComponent<MainCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 c_velocity = _player.current_velocity;
        Vector3 c_rotation = _player.current_rotation;

        text.text = "Direction: "    + _player.dir.ToString();
        text.text += "\nPos: "       + _player.transform.position.ToString();
        text.text += "\nRotation: "  + c_rotation.ToString();
        text.text += "\nVelocity: "  + c_velocity.ToString();
        text.text += "\nMomentum:"     + _player.momentum.ToString();
        text.text += "\nSpecial:"     + _player.special_weapon;

        text.text += "\n\nTouch Debug: ";
        text.text += "\n\nCamera:";
        text.text += "\nPosition:" + _camera.transform.position.ToString();
        text.text += "\nRotation:" + _camera.transform.rotation.ToString();

        text.text += "\n\nReticle Debug: ";
        text.text += "\nLocked Targets: " + _reticle.locked_targets.Count;
        text.text += "\nMax Targets: " + _reticle.getMaxTargets();
        text.text += "\nTarget IDs Length: " + _reticle.locked_ids.Count;

    }
}
