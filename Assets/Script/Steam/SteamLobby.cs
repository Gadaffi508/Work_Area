using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SteamLobby : MonoBehaviour
{
    public CSteamID SteamNumber;
    //Calbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    //variable
    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager manager;

    //Gameobject
    public GameObject HostButton;
    public Text LobbyNameText;
    
    public static CSteamID LobbyID { get; private set; }

    public InputField field;

    private void Start()
    {
        manager = GetComponent<CustomNetworkManager>();
        
        if (!SteamManager.Initialized) return;

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        HostButton.SetActive(false);    
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        Debug.Log("Lobby Created Successfully");

        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        manager.StartHost();

        // Set lobby data including the host address
        SteamMatchmaking.SetLobbyData(LobbyID, HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(LobbyID, "name", SteamFriends.GetPersonaName().ToString() + "'S LOBBY");

        // Add the following line to set the host address in lobby data
        string hostAddress = SteamUser.GetSteamID().ToString();
        SteamMatchmaking.SetLobbyData(LobbyID, HostAddressKey, hostAddress);

        Debug.Log("Host address set in lobby data: " + hostAddress);
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To join  Lobby");

        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    
    public void JoinLobby(ulong lobbyId)
    {
        SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
    }

    // Add the following method to be called when a button for joining a lobby is clicked, for example
    public void JoinButtonClicked()
    {
        string lobbyIdToJoinString = field.text;

        // Check if the entered lobby ID is a valid ulong
        if (ulong.TryParse(lobbyIdToJoinString, out ulong lobbyIdToJoin))
        {
            JoinLobby(lobbyIdToJoin);
        }
        else
        {
            Debug.LogError("Invalid lobby ID entered. Please enter a valid number.");
        }
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) return;
        HostButton.SetActive(false);
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        LobbyNameText.gameObject.SetActive(true);

        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");

        //manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        
        CSteamID hostID = SteamUser.GetSteamID();
        
        if (hostID.IsValid())
        {
            string hostAddress = hostID.ToString();
        
            if (!string.IsNullOrEmpty(hostAddress))
            {
                manager.networkAddress = hostAddress;
                manager.StartClient();
            }
            else
            {
                Debug.LogError("Host address is empty or null. Cannot start client.");
            }
        }
        else
        {
            Debug.LogError("Invalid host ID. Cannot start client.");
        }

    }

    public void ConnectServer()
    {
        
    }
}
