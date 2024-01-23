using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager Instance;
    
    //Lobbies List Variables
    public GameObject lobbyDataItemPrefab;
    public GameObject lobbyListContent;


    public List<GameObject> listOfLobbies = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void GetListOfLobbies()
    {
        SteamLobby.Instance.GetLobbiesList();
    }

    //Lobileri görme
    public void DisaplayLobbies(List<CSteamID> lobbyIds, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIds.Count; i++)
        { 
            //lobileri steamda nokta atışı kontrol etme
            if (lobbyIds[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                //Oluşturulan lobiyi atama
                GameObject createdItem = Instantiate(lobbyDataItemPrefab);
                
                //lobilere veri aktarma
                createdItem.GetComponent<LobbyDataEntry>().lobbyId = (CSteamID)lobbyIds[i].m_SteamID;
                
                createdItem.GetComponent<LobbyDataEntry>().MemebersText.text = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyIds[i].m_SteamID).ToString();

                createdItem.GetComponent<LobbyDataEntry>().lobbyName =
                    SteamMatchmaking.GetLobbyData((CSteamID)lobbyIds[i].m_SteamID, "name");
                
                createdItem.GetComponent<LobbyDataEntry>().SetLobbyData();
                createdItem.transform.SetParent(lobbyListContent.transform);
                createdItem.transform.localScale = Vector3.one;
                
                listOfLobbies.Add(createdItem);
            }
        }
    }
    
    //lobileri temizleme
    public void DestroyLobbies()
    {
        foreach (GameObject lobbyıtem in listOfLobbies)
        {
            Destroy(lobbyıtem);
        }
        listOfLobbies.Clear();
    }
}
