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

    //[SerializeField]
    //private GameObject camera;
    //private Camera _camera;
    //private MainCamera _main;

    string homing_1cd;
    string homing_2cd;
    string homing_3cd;
    string homing_4cd;
    string homing_5cd;
    string homing_6cd;
    string homing_7cd;
    string homing_8cd;

    [SerializeField]
    HomingMissle homing_1;
    [SerializeField]
    HomingMissle homing_2;
    [SerializeField]
    HomingMissle homing_3;
    [SerializeField]
    HomingMissle homing_4;
    [SerializeField]
    HomingMissle homing_5;
    [SerializeField]
    HomingMissle homing_6;
    [SerializeField]
    HomingMissle homing_7;
    [SerializeField]
    HomingMissle homing_8;

    // Start is called before the first frame update
    void Start()
    {
        //_touch = touchHandler.GetComponent<TouchHandler>();

        text = transform.GetComponent<Text>();
        _reticle = reticle.GetComponent<Reticles>();
        _player = player.GetComponent<Player>();
        _sys = gameSystem.GetComponent<GameSystem>();
        //_camera = camera.GetComponent<Camera>();
        //_main = camera.GetComponent<MainCamera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 c_velocity = _player.current_velocity;
        Vector3 c_rotation = _player.current_rotation;

        text.text = "Direction: "    + _player.dir.ToString();
        text.text += "\nVelocity: " + c_velocity.ToString();
        //text.text += "\nPos: "       + _player.transform.position.ToString();
        //text.text += "\nForward: " + _player.transform.forward;
        //text.text += "\nRotation: "  + c_rotation.ToString();
        //text.text += "\nMomentum:"     + _player.momentum.ToString();
        text.text += "\nSpecial:"     + _player.special_weapon;
        text.text += "\nSpecial Level:" + _player.weapon_level_homingMissles;

        //text.text += "\n\nTouch Debug: ";
        //text.text += "\n\nCamera:";
        //text.text += "\nPosition:" + _camera.transform.position.ToString();
        //text.text += "\nRotation:" + _camera.transform.rotation.ToString();

        //text.text += "\n\nReticle Debug: ";
        //text.text += "\nLocked Targets: " + _reticle.locked_targets.Count;
        //text.text += "\nN-Targets: " + _reticle.getNumTargets();
        //text.text += "\nTarget IDs: " + count_locked();
        //text.text += "\nDynamic Raycast Offset: " + _reticle.dynamic_raycast_targetlock_offset;

        text.text += "\n\nMissles: ";

        homing_1cd = homing_1._cooldown > Time.time ? "|" : "O";
        homing_2cd = homing_2._cooldown > Time.time ? "|" : "O";
        homing_3cd = homing_3._cooldown > Time.time ? "|" : "O";
        homing_4cd = homing_4._cooldown > Time.time ? "|" : "O";
        homing_5cd = homing_5._cooldown > Time.time ? "|" : "O";
        homing_6cd = homing_6._cooldown > Time.time ? "|" : "O";
        homing_7cd = homing_7._cooldown > Time.time ? "|" : "O";
        homing_8cd = homing_8._cooldown > Time.time ? "|" : "O";

        text.text += "\n1: " + homing_1cd + "\tTarget:" + homing_1.debug_id;
        text.text += "\nDelta: " + homing_1.rot_amt;
        text.text += "\nForward: " + homing_1.transform.forward;
        text.text += "\nV: " + homing_1.velocity;
        text.text += "\nD: " + homing_1.direction;
        text.text += "\n2: " + homing_2cd + "\tTarget:" + homing_2.debug_id;
        text.text += "\nDelta: " + homing_2.rot_amt;
        text.text += "\nV: " + homing_2.velocity;
        text.text += "\nD: " + homing_2.direction;
        text.text += "\n3: " + homing_3cd + "\tTarget:" + homing_3.debug_id;
        text.text += "\nDelta: " + homing_3.rot_amt;
        text.text += "\nV: " + homing_3.velocity;
        text.text += "\nD: " + homing_3.direction;
        text.text += "\n4: " + homing_4cd + "\tTarget:" + homing_4.debug_id;
        text.text += "\nDelta: " + homing_4.rot_amt;
        text.text += "\nV: " + homing_4.velocity;
        text.text += "\nD: " + homing_4.direction;
        text.text += "\n5: " + homing_5cd + "\tTarget:" + homing_5.debug_id;
        text.text += "\nDelta: " + homing_5.rot_amt;
        text.text += "\nV: " + homing_5.velocity;
        text.text += "\nD: " + homing_5.direction;
        text.text += "\n6: " + homing_6cd + "\tTarget:" + homing_6.debug_id;
        text.text += "\nDelta: " + homing_6.rot_amt;
        text.text += "\nV: " + homing_6.velocity;
        text.text += "\nD: " + homing_6.direction;
        text.text += "\n7: " + homing_7cd + "\tTarget:" + homing_7.debug_id;
        text.text += "\nDelta: " + homing_7.rot_amt;
        text.text += "\nV: " + homing_7.velocity;
        text.text += "\nD: " + homing_7.direction;
        text.text += "\n8: " + homing_8cd + "\tTarget:" + homing_8.debug_id;
        text.text += "\nDelta: " + homing_8.rot_amt;
        text.text += "\nV: " + homing_8.velocity;
        text.text += "\nD: " + homing_8.direction;

    }


    public int count_locked()
    {
        int nnulls = 0;
        for (int i = 0; i < _reticle.locked_ids.Length; i++) {
            if (_reticle.locked_ids[i] != 0) nnulls++;
        }
        return nnulls;
    }
}
