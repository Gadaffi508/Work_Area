using System.Collections.Generic;
using System.Linq;
using Mirror;
using Steamworks;
using UnityEngine;

public class SteamLobbyController : MonoBehaviour
{
    public static SteamLobbyController Instance;
    private void Awake() => Instance = this;
    [Header("Player Item")]
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    [Header("Lobby")]
    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    
    private List<PlayerLıstItem> playerLıstItems = new List<PlayerLıstItem>();
    public SteamPlayerObject localPlayer;
    
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

    public void UpdateLobbyName()
    {
        currentLobbyID = Manager.GetComponent<SteamLobbyManager>().CurrentLobbyID;
    }
    
    public void UpdatePlayerList()
    {
        foreach (SteamPlayerObject player in Manager.GamePlayer)
        {
            Debug.Log(Manager.GamePlayer);
            if (playerLıstItems.Any(item => item.ConnectionId == player.connectionID))
                continue;

            GameObject newPlayerItem = Instantiate(playerListItemPrefab, playerListViewContent.transform);
            PlayerLıstItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerLıstItem>();
            
            newPlayerItemScript.PlayerName = player.playerName;
            newPlayerItemScript.ConnectionId = player.connectionID;
            newPlayerItemScript.PlayerSteamID = player.playerSteamId;
            newPlayerItemScript.SetPlayerValues();
            
            playerLıstItems.Add(newPlayerItemScript);
        }

        List<PlayerLıstItem> playerItemsToRemove = playerLıstItems.Where(item => !Manager.GamePlayer.Any(player => player.connectionID == item.ConnectionId)).ToList();
        foreach (PlayerLıstItem itemToRemove in playerItemsToRemove)
        {
            playerLıstItems.Remove(itemToRemove);
        }
    }
    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayer = localPlayerObject.GetComponent<SteamPlayerObject>();
    }
}
