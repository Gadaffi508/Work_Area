using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookManager : MonoBehaviour
{
    public Transform SrcObj;
    public Transform CurrentPosition;
    public float Radius;

    private bool npcInside;
    private float LerpSpeed = 5f;

    private void Start() => CurrentPosition = transform;

    private void Update()
    {
        Collider[] col = Physics.OverlapSphere(transform.position,Radius);
        npcInside = false;
        foreach (Collider npc in col)
        {
            if (npc != null)
            {
                if (npc.gameObject.CompareTag("NPC"))
                {
                    SrcObj.transform.position = LookLerp(LookOffset(npc.gameObject.transform.position));
                    npcInside = true;
                }
            }
        }

        if (npcInside == false) SrcObj.transform.position = LookLerp(CurrentPosition.position);
    }


    private Vector3 LookOffset(Vector3 NPC) => new Vector3(NPC.x,1,NPC.z);
    
    private Vector3 LookLerp(Vector3 NPC) => Vector3.Lerp(SrcObj.transform.position,NPC,Time.deltaTime * LerpSpeed);
    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, Radius);
}
