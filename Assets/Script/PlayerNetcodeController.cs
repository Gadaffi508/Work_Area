using System;
using UnityEngine;

public class PlayerNetcodeController : MonoBehaviour
{
    public float Speed = 1;
    public float ziplamaGucu = 10f;
    public float yerCekimi = -30f;
    
    private CharacterController characterController;
    
    private Vector3 hareketYonu;
    private float dikeyHiz;

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); 
    }

    private void Update()
    {
        HareketInputunuAl();
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
