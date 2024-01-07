using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : NetworkBehaviour
{
    private NavMeshAgent _agent;
    private Animator anim;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if(!isOwned) return;
        anim.SetFloat("speed",_agent.velocity.magnitude);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(MouseClick(), out hit)) _agent.SetDestination(hit.point);
        }
    }
    private Ray MouseClick() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
