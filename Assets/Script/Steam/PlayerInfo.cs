using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar (hook = nameof(HandleSteamIdUpdated))] 
    private ulong steamID;

    public string displayName;

    #region Server

    public void SetSteamId(ulong steamId)
    {
        this.steamID = steamId;
    }
    #endregion

    #region Client
    
    public void HandleSteamIdUpdated(ulong oldSteamId,ulong newSteamId)
    {
        var cSteamId = new CSteamID(newSteamId);
        displayName = SteamFriends.GetFriendPersonaName(cSteamId);
    }
    #endregion
    
}
