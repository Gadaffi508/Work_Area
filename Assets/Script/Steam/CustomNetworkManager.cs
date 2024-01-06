using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(
            SteamLobby.LobbyID,
            numPlayers - 1);

        var playerInfoDisplay = conn.identity.GetComponent<PlayerInfo>();
        
        playerInfoDisplay.SetSteamId(steamID.m_SteamID);
    }
}


