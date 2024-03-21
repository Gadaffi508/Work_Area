using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamLobbyManager : MonoBehaviour
{
    public static SteamLobbyManager Instance;
    //callbacks
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequest;
    protected Callback<LobbyEnter_t> lobbyEntered;

    protected Callback<LobbyMatchList_t> lobbyList;
    protected Callback<LobbyDataUpdate_t> lobbyData;

    public List<CSteamID> lobbyID = new List<CSteamID>();
    
    //manager
    private MyNetworkManager _manager;

    public ulong CurrentLobbyID;
    public GameObject lobbies;

    private void Start()
    {
        if(!SteamManager.Initialized) return;
        if (Instance == null) Instance = this;
        _manager = GetComponent<MyNetworkManager>();
        
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        lobbyList = Callback<LobbyMatchList_t>.Create(MatchLobby);
        lobbyData = Callback<LobbyDataUpdate_t>.Create(GetLobbyData);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK) return;
        
        Debug.Log("Lobby created");
        lobbies.SetActive(true);
        
        _manager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress",
            SteamUser.GetSteamID().ToString());
        
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
            SteamFriends.GetPersonaName().ToString());
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Requested");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        if(NetworkServer.active) return;

        _manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");
        _manager.StartClient();
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
    }

    public void JoinLobby(CSteamID _lobbyID)
    {
        SteamMatchmaking.JoinLobby(_lobbyID);
    }

    public void FindLobbies()
    {
        if(lobbyID.Count>0) lobbyID.Clear();
        
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
        SteamMatchmaking.RequestLobbyList();
    }

    private void MatchLobby(LobbyMatchList_t callback)
    {
        if (FindLobbyManager.Instance.lobbyList.Count>0) FindLobbyManager.Instance.ClearLobby();

        for (int i = 0; i < callback.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyID.Add(lobbyId);
            SteamMatchmaking.RequestLobbyData(lobbyId);
        }
    }

    private void GetLobbyData(LobbyDataUpdate_t callback)
    {
        FindLobbyManager.Instance.DisplayLobby(lobbyID,callback);
    }
}
