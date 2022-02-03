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
    Vector3 trackPoint;

    // Lock-on reticles are children of the reticle game object. (They could also be prefabs but I think this is more performant)
    private const int lock_reticle_child_index_start = 4;
    private const int lock_reticle_child_index_end = 11;

    // LockReticle Transform
    Queue<Transform> allLockReticles_0;
    Queue<Transform> allLockReticles_1;
    Queue<Transform> allLockReticles_2;
    // Active lockReticle queue
    Queue<Transform> activeLockReticleQueue;

    // Next reticle to be initialized
    LockReticle nextReticle;
    // Modulus controls which old lock is replaced by a new one. It should always be the least recent one
    public int current_queue = 0;

    //private Vector3 reticle_vector_1_pos;
    private Vector3 reticle_vector_2_pos;
    private Vector3 reticle_vector_3_pos;
    private Player player;
    bool tracked = false;

    public Queue<Transform> locked_targets; // This queue is tracking all currently locked on enemies but is currently unused.
    public List<int> locked_ids;            // Enemy object IDs that the player is currently locked onto
    public int max_targets = 2;

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

        allLockReticles_0 = new Queue<Transform>();
        allLockReticles_1 = new Queue<Transform>();
        allLockReticles_2 = new Queue<Transform>();

        for (int i = 0; i < lock_reticle_child_index_end - lock_reticle_child_index_start + 1; i++)
        {
            Transform ret = transform.GetChild(i + lock_reticle_child_index_start);
            if (allLockReticles_0.Count < 2)
                allLockReticles_0.Enqueue(ret);
            if (allLockReticles_1.Count < 4)
                allLockReticles_1.Enqueue(ret);
            if (allLockReticles_2.Count < 8)
                allLockReticles_2.Enqueue(ret);
        }

        cam = _cam.GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Player>();
        homing_reticle_ray = new Vector3(0,0,0);
        //laser_reticle_1 = transform.GetChild(0);
        laser_reticle_2 = transform.GetChild(1);
        laser_reticle_3 = transform.GetChild(2);
        homing_reticle_default = transform.GetChild(3);

        // Start with base upgrade reticle group
        upgradeHomingMissle(0);
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
    private void upgradeHomingMissle(int n) {
        switch (n) {
            case 0:
                activeLockReticleQueue = allLockReticles_0;
                break;
            case 1:
                activeLockReticleQueue = allLockReticles_1;
                break;
            case 2:
                activeLockReticleQueue = allLockReticles_2;
                break;
        }
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
                    if (hit.transform.GetInstanceID() == locked_ids[i]) // This transform is already in our locked_targets queue
                    {
                        tracked = true;
                        return; // target is already being tracked
                    }
                }

                //  If we already have max_targets locked
                if (locked_targets.Count == max_targets && !tracked)
                {
                    // This emulates Dequeue -> Enqueue with less overhead
                    Transform replace_lock_reticle = activeLockReticleQueue.Peek();
                    replace_lock_reticle.gameObject.SetActive(false);
                    locked_targets.Dequeue(); // This queue is tracking all currently locked on enemies but is currently unused.
                    locked_ids.RemoveAt(0);
                }
                CreateLock(hit.transform);
            }
        }
    }

    private void CreateLock(Transform target)
    {
        trackPoint = cam.WorldToScreenPoint(target.transform.position);
        trackPoint.z = 0;

        nextReticle = activeLockReticleQueue.Dequeue().GetComponent<LockReticle>();
        nextReticle.gameObject.SetActive(true);
        nextReticle.target = target;
        activeLockReticleQueue.Enqueue(nextReticle.transform);

        addLockedOnTarget(target.transform);

    }

    public void addLockedOnTarget(Transform target)
    {
        int iid = target.transform.GetInstanceID();
        locked_ids.Add(iid);
        locked_targets.Enqueue(target);
    }

    public void setMaxTargets(int n) { max_targets = n; }
    public int getMaxTargets() { return max_targets; }
    public void TrackTarget()
    {
        
    }
}
