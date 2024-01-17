using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{
    //Callbacks bunlar steam ile haberleşmesini sağlar
    //protected ile koruyoruz
    //lobi oluşturma parametresi veriyoruz
    protected Callback<LobbyCreated_t> LobbyCreated;
    //Oyun lobisine katılma isteği
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    //Oyun Lobisine girme
    protected Callback<LobbyEnter_t> LobbyEntered;
    
    //Lobi oluştuğunda bir kimlik oluşcak
    //Bu ıd arkadaşlar katılma için kullancaz
    public ulong CurrentLobbyID;
    //ana bilgisayar adres anahtarı
    private const string HostAddressKey = "HostAddress";
    //ağ yöneticisine atıfta bulunmak
    private CustomNetworkManager cmanager;
    
    //Oyun Nesnesi
    public GameObject HostButton;
    public Text LobbyNameText;

    private void Start()
    {
        //Steamın çalışıp çalışmadığını kontrol ediyoruz
        if(!SteamManager.Initialized) return;

        cmanager = GetComponent<CustomNetworkManager>();
        
        //calback ları çalıştırma
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    //Lobi Oluşturma
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        //ilk önce lobi düzgün oluştumu kontrol ediyoruz
        if(callback.m_eResult != EResult.k_EResultOK) return;
        
        Debug.Log("Lobby Created Succesfully");
        
        //başlangıç sunucusunu çağırıyoruz
        cmanager.StartHost();
        //oyunun barındırılmasını başlatcak ve callback lar
        
        //lobi verilerini ayarlıcaz bu yüzden lobi verileri üzerinde setlobbydata yapıyoruz
        SteamMatchmaking.SetLobbyData(
            //steam lobi id si
            //direk m_ulSteamIDLobby bunu yazabiliriz ama çevirmemiz lazım onun için new yapıyoruz
            new CSteamID(callback.m_ulSteamIDLobby),
            //buraya değiştirmek istedğimiz verileri yazıyoruz
            //ana bilgisayarı değiştir diyoruz
            HostAddressKey,
            //Ney ile değiştircemizi de buraya yazıyoruz
            //steam kullanıcı kimlik yap diyoruz
            //ve bunu to string yapıyoruz
            SteamUser.GetSteamID().ToString()
            );
        
        //Lobi adı
        SteamMatchmaking.SetLobbyData(
            //tekrar dönüştürüyoruz
            new CSteamID(callback.m_ulSteamIDLobby),
            //değiştircemiz şeyi yazıyoruz
            "name",
            //oyuncunun adı yapıyoruz
            SteamFriends.GetPersonaName().ToString() + " 's LOBBY"
            );
    }
    
    //Lobiye Katılma İsteği
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        //Debug ile katılma isteğini yazdırıyoruz
        Debug.Log("Request To Join Lobyy");

        //lobiye katılma isteme
        //katılacağı lobi id sini parametre veriyoruz
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //herkes için yapılması olan, lobi sahibide olsa da dahil
        //Oyuna girerken yapılcaklar
        HostButton.SetActive(false);
        
        //lobi id sini girdiğimiz lobi id sine eşitliyoruz
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        //lobi adı görmek içim
        LobbyNameText.gameObject.SetActive(true);
        //lobinin adını getiriyoruz
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby ),
            "name"
            );
        
        //Müşteriler için yapılması olan
        //önce müşteri olup olmadığını kontrol edelim
        //ağ sunucusunu aktif hale getiriyoruz
        if(NetworkServer.active) return;
        
        //ağ yöneticisinin adresini ayarlamak
        cmanager.networkAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby ),
            HostAddressKey
            );
        //işlemciyi başlatıyoruz
        cmanager.StartClient();
    }
    
    //Buton için
    public void HostLobby()
    {
        //lobimize ev sahipliği yapıyoruz
        //İlk önce lobi türü daha sonra max insan sayısı
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
    }
}



























