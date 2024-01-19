using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

//Kendi Network Managerımız
public class CustomNetworkManager : NetworkManager
{
    //Her bağlantı için oluşturcak olan oyuncumuz referans
    [SerializeField] private SteamPlayerController gamePlayerPrefabs;

    //Tüm oyuncular
    public List<SteamPlayerController> GamePlayer { get; } = new List<SteamPlayerController>();

    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //Her oyuncu eklendiğinde
        //Doğru sahne olup olmadığını kopntrol ediyoruz
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            //SteamPlayerController örneğini çağırcaz eşit şekilde örneklendircez
            SteamPlayerController GamePlayerInstance = Instantiate(gamePlayerPrefabs);
            //verileri imzalama, nokta bağlantı kimliğne geçiyoruz oyuncu numarasınıda ayarlıyoruz
            GamePlayerInstance.ConnectionId = conn.connectionId;
            GamePlayerInstance.PlayerId = GamePlayer.Count + 1;
            GamePlayerInstance.PlayerSteamId = (ulong)SteamMatchmaking.GetLobbyMemberByIndex(
                (CSteamID)SteamLobby.Instance.CurrentLobbyID,
                GamePlayer.Count
                );
            //bağlı her istemci için oynatıcı eklemek
            NetworkServer.AddPlayerForConnection(conn,GamePlayerInstance.gameObject);
        }
    }

    public void StartGame(string SceneName)
    {
        //Sahne değiştirme
        ServerChangeScene(SceneName);
    }
}


