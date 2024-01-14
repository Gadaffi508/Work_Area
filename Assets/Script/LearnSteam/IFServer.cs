using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class IFServer : MonoBehaviour
{
    private CLobbyListManager _lobbyListManager;
    private void Awake()
    {
        // Steam API'yi başlat
        bool isSteamInitialized = SteamAPI.Init();
        
        // Steam başlatılamazsa, hata mesajını göster veya bir şey yap
        if (!isSteamInitialized)
            Debug.LogError("Steam API initialization failed!");
        
        // CLobbyListManager sınıfını oluştur
        _lobbyListManager = new CLobbyListManager();

        // Lobi listesini bulmak için
        _lobbyListManager.FindLobbies();
    }
    
    // Steam API'yi kapat
    private void OnApplicationQuit() => 
        SteamAPI.Shutdown();
}