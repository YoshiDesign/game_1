using UnityEngine;

public class Reticle : MonoBehaviour
{
    public GameObject player;
    public Vector3 reticlePosition;
    public float reticle_drag;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(100, 10, 50);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = player.transform.rotation;
    }
}
