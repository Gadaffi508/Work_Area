using System;
using UnityEngine;
using Steamworks;

public class SteamLearnWork : MonoBehaviour
{
    //Callback Steamworks'ün en önemli özelliğidir; oyununuzu kilitlemeden Steam'den eşzamansız olarak veri almanıza olanak tanır.
    //Steam Arayüzü her etkinleştirildiğinde veya devre dışı bırakıldığında size bir geri arama gönderir.
    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    
    //CallResults, Geri Aramalara çok benzer ancak bunlar, Geri Aramalar gibi genel bir olay havuzundan ziyade belirli bir işlev çağrısının eşzamansız sonucudur.
    //CallResult sağlayan bir işlevi, dönüş değerini inceleyerek tanımlayabilirsiniz. SteamAPICall_t değerini döndürürse bir CallResult ayarlamanız gerekir.
    private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
    private void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
        }
    }
    
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
            m_NumberOfCurrentPlayers.Set(handle);
            Debug.Log("Called GetNumberOfCurrentPlayers()");
        }
    }

    private void OnEnable()
    {
        //Daha sonra Callback<>.Create()'i çağırıp m_GameOverLayActivated'e atayarak Geri Aramamızı oluştururuz. Bu, geri aramanın çöp toplanmasını engeller.
        //Bunu genellikle OnEnable'da yaparız, çünkü bu, Unity derlemeleri yeniden yükledikten sonra Geri Aramayı yeniden oluşturmamıza olanak tanır.
        if (SteamManager.Initialized) 
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            Debug.Log(" IsGameServer " + m_GameOverlayActivated.IsGameServer);
            Debug.Log(" m_GameOverlayActivated " + m_GameOverlayActivated);
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
        }
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
        
        //GameOverlayActivated Callback'in popüler ve önerilen kullanım durumlarından biri, arayüz açıldığında oyunu duraklatmaktır.
    }
    private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure) 
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure) {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }
    
}
