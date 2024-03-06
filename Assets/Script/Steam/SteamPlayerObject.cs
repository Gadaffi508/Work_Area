using Steamworks;
using Mirror;
using UnityEngine;

public class SteamPlayerObject : NetworkBehaviour
{
    [SyncVar] public int connectionID, playerIdNumber;
    public ulong playerSteamId;

    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName;

    #region Singleton

    private MyNetworkManager _manager;

    private MyNetworkManager Manager
    {
        get
        {
            if (_manager != null)
            {
                return _manager;
            }

            return _manager = MyNetworkManager.singleton as MyNetworkManager;
        }
    }
    #endregion

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        SteamLobbyController.Instance.FindLocalPlayer();
        SteamLobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayer.Add(this);
        SteamLobbyController.Instance.UpdateLobbyName();
        SteamLobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayer.Remove(this);
        SteamLobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string _playerName)
    {
        this.PlayerNameUpdate(this.playerName,_playerName);
    }

    private void PlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer)
        {
            this.playerName = newValue;
        }

        if (isClient)
        {
            SteamLobbyController.Instance.UpdatePlayerList();
        }
    }
}
