using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    public SteamPlayerObject gamePlayerPrefabs;
    public List<SteamPlayerObject> GamePlayer = new List<SteamPlayerObject>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        SteamPlayerObject gamePlayerInstance = Instantiate(gamePlayerPrefabs);
        gamePlayerInstance.connectionID = conn.connectionId;
        gamePlayerInstance.playerIdNumber = GamePlayer.Count + 1;
        gamePlayerInstance.playerSteamId =
                (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobbyManager.Instance.CurrentLobbyID,GamePlayer.Count);

        NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        
    }
}
