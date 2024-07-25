using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SteamScoreBoardManager : MonoBehaviour
{
    #region Singleton

    private MyNetworkManager _manager;

    private MyNetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = NetworkManager.singleton as MyNetworkManager;
        }
    }

    #endregion

    public GameObject playerGetScorePrefab;

    public GameObject scoreMenu;

    public Transform viewContent;

    void Start()
    {
        for (int i = 0; i < Manager.GamePlayer.Count; i++)
        {
            GameObject player = Instantiate(playerGetScorePrefab, viewContent);
            SteamPlayerGetScoreData playerGetScoreData = player.GetComponent<SteamPlayerGetScoreData>();

            playerGetScoreData.playerName = Manager.GamePlayer[i].playerName;
            playerGetScoreData.playerMS = Manager.GamePlayer[i].pingInMS;

            playerGetScoreData.ShowScoreBoard();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            scoreMenu.SetActive(true);
        else if(Input.GetKeyUp(KeyCode.Tab))
            scoreMenu.SetActive(false);
    }
}