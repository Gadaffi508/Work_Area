using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class CameraController : NetworkBehaviour
{
    ///<summary>
    /// Camera Controller
    /// </summary>
    public GameObject cameraHolder;

    public Vector3 offset;

    public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")cameraHolder.transform.position = transform.position + offset;
    }
}
