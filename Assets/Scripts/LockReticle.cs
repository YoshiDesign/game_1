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
        print("LOCK RETICLE STARTING");
        // TODO - Optimize this. Find out how to convert world to screen space manually
        // At least just for the x and y coords.
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //Debug.Log("Reticle Active. Target Dist: " + target.position.z);
        //float target_z = Mathf.Min(3000 / target.position.z, 3.0f);
        rotation = new Vector3(0, 0, 30.0f * Time.deltaTime);
        //_image = transform.GetComponent<RawImage>();
        //Debug.Log(_image.rectTransform.ToString());
        //Debug.Log(_image.rectTransform.sizeDelta);
        //_image.rectTransform.sizeDelta = new Vector2(target_z, target_z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation, Space.Self);
    }

    private void FixedUpdate()
    {
        if (target == null) {
            this.gameObject.SetActive(false);
            return;
        }

        trackPoint = cam.WorldToScreenPoint(target.transform.position);
        trackPoint.z = 0;
        transform.position = trackPoint;
    }
}
