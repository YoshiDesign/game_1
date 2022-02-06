using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockReticle : MonoBehaviour
{
    public int index;
    public Transform target;

    private RawImage _image;
    public Vector3 rotation;
    private Vector3 trackPoint;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // TODO - Optimize this. Find out how to convert world to screen space manually
        // At least just for the x and y coords.
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rotation = new Vector3(0, 0, 30.0f * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation, Space.Self);
    }

    private void OnDisable()
    {
        target = null;
    }

    private void FixedUpdate()
    {
        // Target was destroyed or otherwise out of play
        if (target == null || target.transform.position.z < -1f) {
            // Recreate the target queues (taret_ids and targets)
            transform.parent.GetComponent<Reticles>().ShuffleTargetQueue();
            // Hide this reticle
            this.gameObject.SetActive(false);
            return;
        }

        trackPoint = cam.WorldToScreenPoint(target.transform.position);
        trackPoint.z = 0;
        transform.position = trackPoint;
    }
}
