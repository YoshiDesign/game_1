using System;
using System.Collections.Generic;
using UnityEngine;

public class Reticles : MonoBehaviour
{

    [SerializeField]
    private GameObject lockedReticlePrefab;
    [SerializeField]
    private GameObject _cam;
    private Camera cam;
    RaycastHit hit;
    Ray ray;
    Vector3 homing_reticle_ray;

    Dictionary<int, GameObject> activeLockReticles;

    //private Vector3 reticle_vector_1_pos;
    private Vector3 reticle_vector_2_pos;
    private Vector3 reticle_vector_3_pos;
    private Player player;
    bool tracked = false;
    bool bypass_lock = false;
    public Queue<Transform> locked_targets;
    public List<int> locked_ids;
    public int max_targets = 2;            // Missle count determines max locked_targets

    //private Transform laser_reticle_1;
    private Transform laser_reticle_2;
    private Transform laser_reticle_3;
    private Transform homing_reticle_default;
    private Transform homing_reticle_locked;


    // Start is called before the first frame update
    void Start()
    {
        locked_targets = new Queue<Transform>();
        locked_ids = new List<int>();
        activeLockReticles = new Dictionary<int, GameObject>(); // Instance ID: Reticle

        cam = _cam.GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Player>();
        homing_reticle_ray = new Vector3(0,0,0);
        //laser_reticle_1 = transform.GetChild(0);
        laser_reticle_2 = transform.GetChild(1);
        laser_reticle_3 = transform.GetChild(2);
        homing_reticle_default = transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        
        //reticle_vector_1_pos = cam.WorldToScreenPoint(player.reticle_vector_1);
        reticle_vector_2_pos = cam.WorldToScreenPoint(player.reticle_vector_2);
        reticle_vector_3_pos = cam.WorldToScreenPoint(player.reticle_vector_3);
        //reticle_vector_1_pos.z = 0.0f;
        reticle_vector_2_pos.z = 0.0f;
        reticle_vector_3_pos.z = 0.0f;
        reticle_vector_3_pos.y -= 20.0f;
        //laser_reticle_1.position = reticle_vector_1_pos;
        laser_reticle_2.position = reticle_vector_2_pos;
        laser_reticle_3.position = reticle_vector_3_pos;
    }

    private void FixedUpdate()
    {
        if (homing_reticle_default.gameObject.activeSelf)
        {
            // Reticle screen position
            homing_reticle_default.position = reticle_vector_3_pos;
            LockOnTargets();
        }
    }

    public void EnableHomingReticle()
    {
        homing_reticle_default.gameObject.SetActive(true);
    }

    private void LockOnTargets()
    {

        // Cast a ray from the origin
        ray = cam.ScreenPointToRay(reticle_vector_3_pos);

        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        // Add targets to player.locked_targets <Stack>
        if (Physics.Raycast(ray, out hit))
        {
            // Lock-on
            if (hit.transform.tag == "Enemy")
            {
                tracked = false;

                // Check if we're already tracking this target
                for (int i = 0; i < locked_ids.Count; i++)
                {
                    if (hit.transform.GetInstanceID() == locked_ids[i])
                    {
                        Debug.Log("RETURN. 1");
                        tracked = true;
                        return; // target is already being tracked
                    }
                }

                //  If we already have max_targets locked
                if (locked_targets.Count == max_targets && !tracked)
                {
                    Debug.Log("DESTROY");
                    // Remove the first queued
                    Destroy(activeLockReticles[locked_ids[0]]);
                    activeLockReticles.Remove(locked_ids[0]);
                    locked_targets.Dequeue();
                    locked_ids.RemoveAt(0);
                }
                CreateLock(hit.transform);
            }
        }
    }

    //private void ShuffleIds()
    //{
    //    // Queue.Dequeue but returns the rest
    //    locked_ids = new ArraySegment<int>(locked_ids, 1, locked_ids.Length - 1).Array;
    //}

    private void CreateLock(Transform target)
    {
        Vector3 trackPoint = cam.WorldToScreenPoint(target.transform.position);
        trackPoint.z = 0;
        Debug.Log("Create...");
        LockReticle clone = Instantiate(
            lockedReticlePrefab,
            trackPoint,
            Quaternion.identity
        ).GetComponent<LockReticle>();

        clone.GetComponent<LockReticle>();

        //LockReticle _lock_reticle = clone.GetComponent<LockReticle>();
        clone.gameObject.transform.parent = transform;
        clone.target = target;

        addLockedOnTarget(target.transform, clone);
    }

    public void addLockedOnTarget(Transform target, LockReticle lock_reticle)
    {
        int iid = target.transform.GetInstanceID();
        locked_ids.Add(iid);
        locked_targets.Enqueue(target);
        activeLockReticles.Add(iid, lock_reticle.gameObject);
    }

    public void setMaxTargets(int n) {max_targets = n;}
    public int getMaxTargets(){return max_targets;}

    public void TrackTarget()
    {
        
    }
}
