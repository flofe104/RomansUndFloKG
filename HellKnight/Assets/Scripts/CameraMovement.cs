using System.Collections;
using System.Collections.Generic;
using Testing;
using UnityEngine;

[TestMonoBehaviour]
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


    [Test]
    public void TestInitialCameraDistance()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        Assert.ApproxEqual(transform.position.y - playerPos.y, 5, 0.2f);
        Assert.ApproxEqual(transform.position.z - playerPos.z, -20, 0.2f);
    }

}
