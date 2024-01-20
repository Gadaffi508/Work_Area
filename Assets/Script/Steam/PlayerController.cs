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
    
    private Vector3 hareketYonu;
    private float dikeyHiz;
    //Oyuncu objemiz
    public GameObject PlayerModel;

    private void Start()
    {
        PlayerModel.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (PlayerModel.activeSelf == false)
            {
                SetPosition();
                characterController = GetComponent<CharacterController>();
                PlayerModel.SetActive(true);
            }
            if(authority) HareketInputunuAl();
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-15, 7));
    }

    void HareketInputunuAl()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(yatay, 0f, dikey) * Speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
