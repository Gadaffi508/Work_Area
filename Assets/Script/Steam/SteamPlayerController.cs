using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

//NetworkBehaviour dan kalıtım alıyoruz network ta mononobehavior dan alıyor
//Böyle yaparak sunucuda senkronize ediyoruz
public class SteamPlayerController : NetworkBehaviour
{
    //Player Data
    //Senkronize
    [SyncVar] 
    public int ConnectionId;
    [SyncVar] 
    public int PlayerId;
    [SyncVar] 
    public ulong PlayerSteamId;
    
    //değişken değiştiğinde fonksiyon çalışcak
    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string PlayerName;
    
    //Hazır olup olmadığını kontrol eden func
    [SyncVar(hook = nameof(PlayerReadyUpdate))]
    public bool Ready;
    
    //Özel ağ yöneticisi referans
    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayerReadyUpdate(bool oldValue, bool newValue)
    {
        if (isServer) // Host
        {
            this.Ready = newValue;
        }
        
        if (isClient)// Client
        {
            SteamLobbyController.Instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CMdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready,!this.Ready);
    }

    public void ChangeReady()
    {
        //yetkimiz varmı kotnrol ediyoruz
        if (authority) CMdSetPlayerReady();
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        SteamLobbyController.Instance.FindLocalPlayer();
        SteamLobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayer.Add(this);
        SteamLobbyController.Instance.UpdateLobbyName();
        SteamLobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayer.Remove(this);
        SteamLobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName,PlayerName);
    }

    public void PlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer) // Host
        {
            this.PlayerName = newValue;
        }
        
        if (isClient)// Client
        {
            SteamLobbyController.Instance.UpdatePlayerList();
        }
    }
    
    //Start Game
    public void CanStartGame(string SceneName)
    {
        //yetkimiz var mı kontrol ediyoruz
        if (authority)
        {
            CmdStartGame(SceneName);
        }
    }
    
    //Bağlı olan her istemci de çalışıcak
    [Command]
    public void CmdStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }
}













