using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerJump playerJump;
    public bool isDoubleJump = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerJump = GetComponent<PlayerJump>();
    }


    void FixedUpdate()
    {
        doubleJump();
    }

    public void doubleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerJump.isJump == true && isDoubleJump == false && playerJump.jumpTimes == 1 && playerJump.nextJumpTime <= 0)
        {
            playerJump.jumpTimes++;
            isDoubleJump = true;
            _rigidbody.AddForce(Vector3.up * playerJump.jumpForce * 2, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerJump.isJump = false;
            isDoubleJump = false;
            playerJump.jumpTimes = 0;
        }
    }
}
