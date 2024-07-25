using System;
using Steamworks;
using Mirror;
using UnityEngine;

public class SteamPlayerObject : NetworkBehaviour
{
    [SyncVar] public GameObject playerModel;
    [SyncVar] public GameObject playerCamera;

    [SyncVar] public int connectionID, playerIdNumber;
    [SyncVar] public ulong playerSteamId;

    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName;

    private Transform _camera;
    

    #region Singleton

    private MyNetworkManager _manager;

    private MyNetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = NetworkManager.singleton as MyNetworkManager;
        }
    }

    #endregion

    public int pingInMS;

    private float NetworkPing => (float)NetworkTime.rtt;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    void Update()
    {
        if(!NetworkClient.isConnected) return;

        pingInMS = Mathf.RoundToInt(NetworkPing * 1000);
    }

    public override void OnStartAuthority()
    {
        DontDestroyOnLoad(this);
        SetPlayerName(SteamFriends.GetPersonaName().ToString());
        
        _camera.rotation = Quaternion.LookRotation(Vector3.back);
        _camera.SetParent(transform);
        _camera.position = new Vector3(0,1.6f,3.67f);
        
        gameObject.name = "LocalGamePlayer";
        SteamLobbyController.Instance.FindLocalPlayer();
        SteamLobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayer.Add(this);
        SteamLobbyController.Instance.UpdateLobbyName();
        SteamLobbyController.Instance.UpdatePlayerLıst();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayer.Remove(this);
        SteamLobbyController.Instance.UpdatePlayerLıst();
    }

    [Command]
    void SetPlayerName(string _playerName)
    {
        this.PlayerNameUpdate(this.playerName, _playerName);
        SteamLobbyController.Instance.UpdatePlayerLıst();
    }

    public void PlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer)
        {
            this.playerName = newValue;
        }
        if(isClient)
        {
            SteamLobbyController.Instance.UpdatePlayerLıst();
        }
    }

    public void CanStartGame(string sceneName)
    {
        CmdStartGame(sceneName);
    }

    [Command]
    void CmdStartGame(string sceneName)
    {
        _manager.StartGame(sceneName);
        
        if(!isLocalPlayer) return;

        playerCamera.SetActive(true);
        playerModel.SetActive(true);
    }
}