using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.OnScreen;
//using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    //Player player;
    //private float can_fire = -1.0f;
    //private float default_fire_rate = 0.5f;

    //private Controller controller;
    // private Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        //gamepad = null;
        //if (Gamepad.current == null)
        //    return; // No gamepad connected.
        //else
        //{
        //    gamepad = Gamepad.current;
        //    Debug.Log("Gamepad Detected");
        //}

        Debug.Log("Player Controller");
        //player = GameObject.Find("Player").GetComponent<Player>();

//#if UNITY_ANDROID
//        Debug.Log("Android");
//        controller = new AndroidController(player);
//#else
//        Debug.Log("Default");
//        controller = new KeyboardController(player);
//#endif

    }

    // Update is called once per frame
    void Update()
    {
        //if (Gamepad.current != null && gamepad == null) {
        //    Debug.Log("Gamepad from Update!");
        //    gamepad = Gamepad.current;
        //}

    }

}
//public abstract class Controller 
//{
//    public Player player;
//    public ShootCommand shoot;
//    public MoveCommand move;

//    public Controller(Player _player) => player = _player;
//    public abstract void Update();
//}
//public class AndroidController : Controller 
//{
//    private Player player;
//    public AndroidController(Player _player) : base(_player) {
//        player = _player;
//    }
//    public override void Update() {
    
//        // If (andoid specific button) {
//        //      player.doStuff();
//        // }
    
//    }
//}

//public class IosController : Controller
//{ 
//    private Player player;
//    public IosController(Player _player) : base(_player)
//    {
//        player = _player;
//    }
//    public override void Update() {}
//}

//public class KeyboardController : Controller
//{
//    private Player player;
//    public KeyboardController(Player _player) : base(_player)
//    {
//        player = _player;
//    }
//    public override void Update() {
        
//    }
//}

//public class GamepadController : Controller
//{
//    private Player player;
//    public GamepadController(Player _player) : base(_player)
//    {
//        player = _player;
//    }
//    public override void Update() { }
//}

//public abstract class Command {
//    public abstract void execute();
//}

//public class ShootCommand : Command {
//    public override void execute()
//    {
        
//    }
//}

//public class MoveCommand : Command
//{
//    public MoveCommand(Vector2 location) { 
    
//    }
//    public override void execute()
//    {

//    }
//}