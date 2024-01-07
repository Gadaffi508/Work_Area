using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerNetcodeController : NetworkBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 45f;
    
    private Animator anim;
    private Rigidbody rb;
    private Vector3 moveDir;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(isOwned) return;
        
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
        
        anim.SetFloat("speed", rb.velocity.magnitude);
        
        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
       
        rb.velocity = moveDir * speed * Time.deltaTime;
    }
}
