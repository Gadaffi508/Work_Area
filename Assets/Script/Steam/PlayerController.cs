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
    public float ziplamaGucu = 10f;
    public float yerCekimi = -30f;
    
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
        transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-15, 7));
    }
    
    void HareketInputunuAl()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");

        hareketYonu = (transform.forward * dikey) + (transform.right * yatay);

        // Yerçekimini uygula
        if (characterController.isGrounded)
        {
            dikeyHiz = -0.5f; // Yerdeyken dikey hız sıfırlanır
            if (Input.GetButtonDown("Jump"))
            {
                dikeyHiz = ziplamaGucu;
            }
        }
        else
        {
            dikeyHiz += yerCekimi * Time.deltaTime;
        }

        HareketEttir();
    }

    void HareketEttir()
    {
        Vector3 hareket = hareketYonu * Speed * Time.deltaTime;
        hareket.y = dikeyHiz;

        characterController.Move(hareket);
    }
}
