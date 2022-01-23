using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 1000.0f;
    public float max_dist = 500.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,0, speed * Time.deltaTime), Space.Self);

        if (transform.position.z > max_dist)
        {
            Destroy(gameObject);
        }
        else {; }
    }
}
