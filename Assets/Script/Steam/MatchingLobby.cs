using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class MatchingLobby : MonoBehaviour
{
    public static MatchingLobby Instance;

    public void Awake() => Instance = this;

    public GameObject lobbyDataPrefab;
    public Transform lobbiesMenuContent;
    
    public List<GameObject> lobbyList = new List<GameObject>();

    public void MatchLobby()
    {
        SteamLobbyManager.Instance.MatchLobbies();
    }
    
    public void DisplayLobby(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDs.Count; i++)
        {
            if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                GameObject lobby = Instantiate(lobbyDataPrefab,lobbiesMenuContent);
                LobbyData data = lobby.GetComponent<LobbyData>();
                data.lobbyID = (CSteamID)lobbyIDs[i].m_SteamID;
                data.lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID,"name");
                data.lobbyMemembers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyIDs[i]);
                
                data.SetLobbyData();
                lobbyList.Add(lobby);
            }
        }
    }

    public void ClearLobbies()
    {
        foreach (GameObject lobby in lobbyList)
        {
            Destroy(lobby);
        }
        lobbyList.Clear();
    }
}
