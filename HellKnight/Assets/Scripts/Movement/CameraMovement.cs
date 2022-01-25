using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 2;
    private Vector3 offset = new Vector3(0, 5, -20);
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += 2 * Time.deltaTime * (player.transform.position + offset - transform.position); 
    }
}
