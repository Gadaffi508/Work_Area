using System;
using UnityEngine;

public class PlayerNetcodeController : MonoBehaviour
{
    public float X, Z;

    public float speed;
    private Rigidbody rb;
    private Animator anim;
    private Quaternion targetRotation;
    private Vector3 movement;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");

        movement = new Vector3(X,0,Z);
        
        anim.SetFloat("speed",rb.velocity.magnitude);
    }

    public void FixedUpdate()
    {
        if (movement != Vector3.zero)
        {
            rb.velocity = new Vector3(movement.normalized.x * speed, rb.velocity.y, movement.normalized.z * speed);
       
            targetRotation = Quaternion.LookRotation(movement);

            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);

            rb.MoveRotation(targetRotation);
        }
    }
}
