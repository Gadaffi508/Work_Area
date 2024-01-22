using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : NetworkBehaviour
{
    public float Speed = 1;
    
    private CharacterController characterController;

    [SyncVar]
    private Vector3 movement;
    //Oyuncu objemiz
    public GameObject PlayerModel;

    private void Start()
    {
        PlayerModel.SetActive(false);
    }

    private void Update()
    {
        if (!authority) return;
        
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (PlayerModel.activeSelf == false)
            {
                SetPosition();
                characterController = GetComponent<CharacterController>();
                characterController.enabled = false;
                PlayerModel.SetActive(true);
            }

            CmdMove();
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-15, 7));
    }

    [Command]
    void CmdMove()
    {
        MoveInput();
    }

    [ClientRpc]
    void MoveInput()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");

        movement = new Vector3(yatay, 0f, dikey) * Speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
