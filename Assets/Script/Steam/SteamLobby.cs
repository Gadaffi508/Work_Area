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

    private void Start()
    {
        if (!SteamManager.Initialized) return;

        manager = GetComponent<CustomNetworkManager>();

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        Debug.Log("Lobby Created Succesfully");

        manager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
            SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
            SteamFriends.GetPersonaName().ToString() + "'S LOBBY");
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To join  Lobby");

        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        HostButton.SetActive(false);
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        LobbyNameText.gameObject.SetActive(true);

        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");
        if (NetworkServer.active)
        {
            SceneManager.LoadScene(1);
        }

        manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        Debug.Log("Load scene");
        manager.StartClient();

    }
}
