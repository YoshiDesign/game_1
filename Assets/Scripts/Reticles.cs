using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public class Reticles : MonoBehaviour
{

    [SerializeField]
    private GameObject _cam;
    private Camera cam;
    RaycastHit hit;
    Ray ray;
    Vector3 homing_reticle_ray;
    Vector3 trackPoint;

    Vector3 init_raycast_targetlock_offset;
    public Vector3 dynamic_raycast_targetlock_offset;

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
    public int[] locked_ids;            // Enemy object IDs that the player is currently locked onto

    //private Transform laser_reticle_1;
    private Transform laser_reticle_2;
    private Transform laser_reticle_3;
    private Transform homing_reticle_default;
    private Transform homing_reticle_locked;

    // Start is called before the first frame update
    void Start()
    {
        locked_targets = new Queue<Transform>();
        locked_ids = new int[Helpers.getMaxHomingTargets()];
        init_raycast_targetlock_offset = new Vector3(-12f, -12f, 0f);
        dynamic_raycast_targetlock_offset = new Vector3(0, 0, 0);

        /**
         * TODO
         * Combine with homing missle Queue instantiation in Player class
         */
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
        /**
         * TODO
         */

        cam = _cam.GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Player>();
        homing_reticle_ray = new Vector3(0,0,0);

        //laser_reticle_1 = transform.GetChild(0);
        laser_reticle_2 = transform.GetChild(1);
        laser_reticle_3 = transform.GetChild(2);
        homing_reticle_default = transform.GetChild(3);

        // Start with base upgrade reticle group
        upgradeHomingMissle(Helpers.HOMING_L0);
    }

    // Update is called once per frame
    void LateUpdate()
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

    private void Update()
    {
        if (homing_reticle_default.gameObject.activeSelf)
        {
            // Reticle screen position
            homing_reticle_default.position = reticle_vector_3_pos;
            LockOnTargets();
        }
    }

    private void FixedUpdate()
    {

    }

    public void EnableHomingReticle()
    {
        homing_reticle_default.gameObject.SetActive(true);
    }

    private void LockOnTargets()
    {

        dynamic_raycast_targetlock_offset.x = (int)(dynamic_raycast_targetlock_offset.x + 1) % 12;
        dynamic_raycast_targetlock_offset.y = (int)(dynamic_raycast_targetlock_offset.x + 1) % 12;

        // This ray is constantly tracing a + pattern in reticle_3
        ray = cam.ScreenPointToRay(reticle_vector_3_pos + init_raycast_targetlock_offset + dynamic_raycast_targetlock_offset);

        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        // Add targets to locked_targets <Queue>
        if (Physics.Raycast(ray, out hit))
        {
            // Lock-on
            if (hit.transform.tag == "Enemy")
            {
                tracked = false;

                // Check if we're already tracking this target
                for (int i = 0; i < locked_ids.Length; i++)
                {
                    if (hit.transform.GetInstanceID() == locked_ids[i]) // This transform is already in our locked_targets queue
                    {
                        tracked = true;
                        return; // target is already being tracked
                    }
                }

                //  If we already have Helpers.max_homing_targets locked
                if (locked_targets.Count == Helpers.getMaxHomingTargets() && !tracked)
                {

                    // This emulates Dequeue -> Enqueue with less overhead
                    Transform replace_lock_reticle = activeLockReticleQueue.Peek();
                    replace_lock_reticle.gameObject.SetActive(false);   // This will also set this reticle's target to null, which is excruciatingly necessary for the succes of CreateLock()
                    
                    locked_targets.Dequeue(); // This queue is tracking all currently locked on enemies but is currently unused.
                    locked_ids = new ArraySegment<int>(locked_ids, 1, locked_ids.Length - 1).Array; // Pop off the front (dequeue)
                    
                }
                CreateLock(hit.transform);
            }
        }
    }

    /**
    * Called by the player. Don't use this function directly
    */
    public void upgradeHomingMissle(int n)
    {
        Helpers.setMaxHomingTargets(n);

        if (locked_targets.Count > 0)
        {
            // Reinitialize the id's array
            locked_ids = new int[n];

            int i = 0;
            foreach (Transform tar in locked_targets)
            {
                locked_ids[i] = tar.GetInstanceID();
                
                i++;
            }
        }

        switch (n)
        {
            case Helpers.HOMING_L0:
                activeLockReticleQueue = allLockReticles_0;
                break;
            case Helpers.HOMING_L1:
                activeLockReticleQueue = allLockReticles_1;
                break;
            case Helpers.HOMING_L2:
                activeLockReticleQueue = allLockReticles_2;
                break;
        }
    }

    // Synchronization of queue and id's as well as cleaning up null targets
    public void ShuffleTargetQueue()
    {
        // Recreate the locked target queue
        locked_targets = new Queue<Transform>(locked_targets.Where(x => x != null));

        // Recreate the id array
        locked_ids = new int[Helpers.getMaxHomingTargets()];
        int i = 0;
        foreach (Transform tar in locked_targets) {
            locked_ids[i++] = tar.GetInstanceID();
        }

    }

    private void CreateLock(Transform target)
    {
        trackPoint = cam.WorldToScreenPoint(target.transform.position);
        trackPoint.z = 0;
        bool _exit = false;

        do { // This loop depends on every reticle's .target being set to null during OnDisable()

            // Dequeue and activate the first free reticle we have
            nextReticle = activeLockReticleQueue.Dequeue().GetComponent<LockReticle>();

            if (nextReticle.target == null)
            {
                nextReticle.gameObject.SetActive(true);
                nextReticle.target = target;
                _exit = true;
            }

            activeLockReticleQueue.Enqueue(nextReticle.transform);

            // Re-Enqueue occupied reticles, including the new one we just acquired
        } while (_exit == false);

        addLockedOnTarget(target.transform);

    }

    public void addLockedOnTarget(Transform target)
    {

        // Push the latest enemy uid
        for (int i = 0; i < locked_ids.Length; i++) {
            if (i == 0) { 
                locked_ids[i] = target.transform.GetInstanceID();
                break;
            }
        }
        
        // Queue the enemy Transform
        locked_targets.Enqueue(target);
    }

    public int getNumTargets() { return locked_targets.Count; }
}
