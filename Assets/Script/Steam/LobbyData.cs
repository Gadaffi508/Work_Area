using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyData : MonoBehaviour
{
    public CSteamID lobbyID;
    public string lobbyName;
    public int lobbyMemmbers;

    public Text lobbyNameText;
    public Text lobbyMemmbersText;

    public void SetLobbyData()
    {
        if (lobbyName == "") lobbyNameText.text = "Null";
        else lobbyNameText.text = lobbyName;

        lobbyMemmbersText.text = lobbyMemmbers + " /10";
    }

    public void JoinLobby()
    {
        SteamLobbyManager.Instance.JoinLobby(lobbyID);
    }
}
