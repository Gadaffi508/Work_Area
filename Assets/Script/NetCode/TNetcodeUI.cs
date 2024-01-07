using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TNetcodeUI : MonoBehaviour
{
    public Button StartHost;
    public Button StartClient;
    public Button QuıtButtons;

    public GameObject UıButtons;

    private void Awake()
    {
        StartHost.onClick.AddListener( () =>
            {
                NetworkManager.Singleton.StartHost();
                UıButtons.SetActive(false);
            });
        StartClient.onClick.AddListener( () =>
        {
            NetworkManager.Singleton.StartClient();
            UıButtons.SetActive(false);
        });
        
        QuıtButtons.onClick.AddListener( () =>
        {
            Application.Quit();
        });
    }
}
