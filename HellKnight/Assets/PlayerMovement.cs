using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalTranslation = Input.GetAxis("Horizontal");
        bool jumpTriggered = Input.GetAxis("Vertical") != 0;

        //Debug.Log("Moving player by " + horizontalTranslation);
        if (horizontalTranslation != 0)
        {
            Move(Vector3.right * horizontalTranslation * Time.deltaTime);
        }
        if(jumpTriggered)
        {
            Jump(Vector3.up);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isJumping && collision.collider.name == "Ground")
        {
            //Debug.Log("Collision with Ground!");
            isJumping = false;
        }
    }

    void Jump(Vector3 dir)
    {
        if (!isJumping)
        {
            //Debug.Log("Jumping");
            GetComponent<Rigidbody>().velocity = dir * jumpPower;
            isJumping = true;
        }
    }

    void Move(Vector3 dir)
    {
        transform.Translate(dir * speed);
    }
    CharacterController controller;
    public float speed;
    public float jumpPower;
    private bool isJumping = false;// TODO: add to documentation


}
