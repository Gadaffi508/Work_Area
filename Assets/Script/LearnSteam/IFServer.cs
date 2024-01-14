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

//Oyun içi bir nesne objesine atancak
public class CLobbyListManager : MonoBehaviour
{
    // Steam'den dönen lobby eşleşme listesi için çağrı sonuçları
    private static CallResult<LobbyMatchList_t> m_CallResultLobbyMatchList;
    // CLobbyListManager örneği için çağrı sonuçları
    private static CallResult<CLobbyListManager> m_CallResultLobbyListManager;
    // Steam'den alınan lobby eşleşme listesi
    private LobbyMatchList_t _pLobbyMatchList;

    // Unity'nin Start fonksiyonu, oyun nesnesi oluşturulduktan sonra otomatik olarak çağrılır.
    void Start()
    {
        // Lobi listesini bulma işlemini başlat
        FindLobbies();
    }

    // Lobileri bulma işlemini gerçekleştiren fonksiyon
    public void FindLobbies()
    {
        // Steam API'si aracılığıyla lobi listesini talep et ve bu işlemi temsil eden bir SteamAPICall_t nesnesini al
        SteamAPICall_t hSteamAPICall = SteamMatchmaking.RequestLobbyList();
        // Çağrı sonuçlarına bu SteamAPICall_t nesnesini ve işlem sonuçlarını ele alacak bir fonksiyonu bağla
        m_CallResultLobbyMatchList.Set(hSteamAPICall, OnLobbyMatchList);
    }

    // Lobi eşleşme listesi alındığında çağrılan fonksiyon
    private static void OnLobbyMatchList(LobbyMatchList_t pLobbyMatchList, bool bIOFailure)
    {
        // Steam üzerinden gelen lobi listesini işleyen işlemler
        // pLobbyMatchList içinde lobilerin bilgileri bulunacaktır.
        //Debug bilgi alma
        Debug.Log("pLobbyMatchList : "
                  + pLobbyMatchList.m_nLobbiesMatching);
        Debug.Log("bIOFailure : "
                  + bIOFailure);
    }
}
