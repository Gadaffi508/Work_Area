using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class CLobbyListManager : MonoBehaviour
{
    // Steam'den dönen lobby eşleşme listesi için çağrı sonuçları
    private static CallResult<LobbyMatchList_t> m_CallResultLobbyMatchList;
    // CLobbyListManager örneği için çağrı sonuçları
    private static CallResult<CLobbyListManager> m_CallResultLobbyListManager;
    // Steam'den alınan lobby eşleşme listesi
    private LobbyMatchList_t _pLobbyMatchList;

    // Unity'nin Start fonksiyonu, oyun nesnesi oluşturulduktan sonra otomatik olarak çağrılır.
    void Start()
    {
        // Lobi listesini bulma işlemini başlat
        FindLobbies();
    }

    // Lobileri bulma işlemini gerçekleştiren fonksiyon
    public void FindLobbies()
    {
        /* Oyununuzun lobi araması olmasını için ISteamMatchmaking::RequestLobbyList
        'i çağırmanız gerekmektedir. Bu işlev eşzamansızdır ve talebinizin durumunu takip etmek için bir SteamAPICall_t kullanımını döndürür.*/
        SteamAPICall_t hSteamAPICall = SteamMatchmaking.RequestLobbyList();
        // Çağrı sonuçlarına bu SteamAPICall_t nesnesini ve işlem sonuçlarını ele alacak bir fonksiyonu bağla
        m_CallResultLobbyMatchList.Set(hSteamAPICall, OnLobbyMatchList);
    }
    
    //Steam arka ucuna bağlantılarına bağlı olarak bu çağrının tamamlanması 300ms'den 5 saniye kadar sürebilir ve 20 saniyelik bir zaman aşımı süresi vardır.
    //Dönen sonuçların sayısı LobbyMatchList_t arama sonucundadır
    //m_nLobbiesMatching	uint32	Number of lobbies that matched search criteria and we have Steam IDs for.
    //daha sonra bunların hepsini tekrarlamak ve ID'lerini almak için ISteamMatchmaking::GetLobbyByIndex'i kullanabilirsiniz.
    CSteamID GetLobbyByIndex(int iLobby)
    {
        // steamda => CSteamID GetLobbyByIndex( int iLobby ); steamda 
        return SteamMatchmaking.GetLobbyByIndex(iLobby);
    }
    //En fazla 50 sonuca kadar döndürülebilir; ancak genellikle bu sayı birkaçı geçmez.
    //Sonuçlar, coğrafi mesafeye göre sıralanan ve yakınındaki herhangi bir filtreye bağlı olarak döndürülür.
    //Varsayılan olarak tamamen dolu lobileri ve yakınlık ayarı k_ELobbyDistanceFilterDefault (civarında) ayarlanmış olanları döndürmeyeceğiz.
    //Bu filtreleri eklemek için RequestLobbyList'i çağırmadan önce bir veya birkaç filtreleme işlevini çağırmalısınız:

    // Lobi eşleşme listesi alındığında çağrılan fonksiyon
    private static void OnLobbyMatchList(LobbyMatchList_t pLobbyMatchList, bool bIOFailure)
    {
        // Steam üzerinden gelen lobi listesini işleyen işlemler
        // pLobbyMatchList içinde lobilerin bilgileri bulunacaktır.
        //Debug bilgi alma
        Debug.Log("pLobbyMatchList : "
                  + pLobbyMatchList.m_nLobbiesMatching);
        Debug.Log("bIOFailure : "
                  + bIOFailure);
    }
}
