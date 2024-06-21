using System;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamLeaveGame : MonoBehaviour
{
    public int sceneID;

    public CSteamID lobbyID ;
    
    #region Singleton

    private MyNetworkManager _manager;
    private MyNetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    #endregion

    void OnEnable()
    {
        lobbyID = (CSteamID)SteamLobbyManager.Instance.CurrentLobbyID;
    }

    public void LeaveGame()
    {
        if(lobbyID != (CSteamID)0)
            SteamLobbyManager.Instance.LeaveLobby(lobbyID);
        else
            Debug.Log("Looby ID : " + lobbyID);
        
        Manager.StopHost();

        Manager.networkAddress = "";
        Manager.StopClient();
        
        SceneManager.LoadScene(sceneID);
    }
}
