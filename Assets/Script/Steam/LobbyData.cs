using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class LobbyData : MonoBehaviour
{
    public CSteamID lobbyID;
    public string lobbyName;
    public int lobbyMemembers;
    public Text lobbyNameText;
    public Text lobbyMemembersText;

    public void SetLobbyData()
    {
        if (lobbyName == "")
            lobbyNameText.text = "null";
        else
            lobbyNameText.text = lobbyName;

        lobbyMemembersText.text = lobbyMemembers+" / 10";
    }

    public void JoinLobby()
    {
        SteamLobbyManager.Instance.JoinLobby(lobbyID);
    }
}
