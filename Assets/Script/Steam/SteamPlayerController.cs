using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SteamPlayerController : NetworkBehaviour
{
    public float speed;
    public float rotationSpeed;

    private float X, Z;
    private Vector3 _movement;
    
    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(!isLocalPlayer) return;
        
        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");
        _animator.SetFloat("speed",_rigidbody.velocity.magnitude);
        
        CmdMove(X,Z);
    }
    
    [Command]
    private void CmdMove(float x, float y) => RpcMove(x,y);

    [ClientRpc]
    private void RpcMove(float x, float y)
    {
        Vector3 direction = new Vector3(x,0,y).normalized;
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        
        _rigidbody.velocity = direction * speed * Time.deltaTime;
    }
}
