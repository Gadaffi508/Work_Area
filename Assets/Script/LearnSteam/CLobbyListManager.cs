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
    
    //Bir dizgi karşılaştırma filtresi ekler.
    void AddRequestLobbyListStringFilter(char pchKeyToMatch, char pchValueToMatch, ELobbyComparison eComparisonType )
    {
        //pchKeyToMatch: Bu parametre, karşılaştırma yapılacak dizginin anahtarıdır. Bu, bir oda özelliğinin adını veya etiketini temsil eder.
        //pchValueToMatch: Bu, karşılaştırılacak dizginin değeridir. Örneğin, bir oda etiketi veya adı.
        //eComparisonType: Bu parametre, karşılaştırma türünü belirler. Örneğin, eşit olma, içermeyen, eksiksiz eşleşme gibi.
        // Dizgi filtresi ekle
        SteamMatchmaking.AddRequestLobbyListStringFilter("gameMode", "CaptureTheFlag", ELobbyComparison.k_ELobbyComparisonEqual);
    }
    
    //Bir sayısal karşılaştırma filtresi ekler.
    void AddRequestLobbyListNumericalFilter(char pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType )
    {
        //pchKeyToMatch: Karşılaştırma yapılacak sayısal özelliğin anahtarı.
        //nValueToMatch: Karşılaştırılacak sayısal değer.
        //eComparisonType: Karşılaştırma türü, örneğin, eşit olma, büyük olma, küçük olma gibi.
        // Sayısal filtresi ekle
        SteamMatchmaking.AddRequestLobbyListNumericalFilter("playerCount", 4, ELobbyComparison.k_ELobbyComparisonEqual);
    }
    
    //Sonuçları, belirtilen değere en yakın şekilde sıralar.
    void AddRequestLobbyListNearValueFilter(char pchKeyToMatch, int nValueToBeCloseTo )
    {
        // pchKeyToMatch: Eşleşecek filtre anahtarı adı. Bu, k_nMaxLobbyKeyLength'ten daha uzun olamaz.
        //nValueToBeCloseTo : Lobilerin sıralanacağı değer.
        SteamMatchmaking.AddRequestLobbyListNearValueFilter("A",1);
    }

    // Sadece belirli sayıda katılmaya yer olan sayıdaki lobilerin dönüşünü filtreler.
    void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable)
    {
        //nSlotsAvailable : Açık olması gereken açık slot sayısı.
        SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(2);
        //Yalnızca belirtilen sayıda açık yuvaya sahip lobileri geri döndürecek şekilde filtreler.
    }

    //Hangi lobiyi arayacağımızı fiziksel mesafeye göre belirler. Bu, Steam arka ucundaki kullanıcının IP adresine ve IP konum haritasına göre belirlenir.
    void AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter)
    {
        SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterClose);
        //Lobileri aramamız gereken fiziksel mesafeyi ayarlar; bu, kullanıcının IP adresine ve Steam destekli IP konum haritasına bağlıdır
    }

    // Döndürülecek maksimum sayıdaki lobiyi belirtir. Bu sayının az olması lobi arama sonuçlarını ve istemciye detaylarını daha hızlı işler.
    void AddRequestLobbyListResultCountFilter(int cMaxResults)
    {
        SteamMatchmaking.AddRequestLobbyListResultCountFilter(1);
        //Geri dönecek maksimum lobi sayısını ayarlar. Sayı ne kadar düşük olursa, lobi sonuçlarının ve ayrıntılarının müşteriye indirilmesi o kadar hızlı olur.
    }
    
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

    //Lobi oluşturmak
    public void CreateLobby()
    {
        //ELobbyType Bu lobinin türü ve görünürlüğü. Bu daha sonra SetLobbyType aracılığıyla değiştirilebilir.
        //cMaxMembers Bu lobiye katılabilecek maksimum oyuncu sayısı. Bu 250'nin üzerinde olamaz.
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic,2);
        Debug.Log("Lobby Created");
    }
    
    //Lobiye katılma
    public void EnterLobby(CSteamID _steamIDLobby)
    {
        //Mevcut bir lobiye katılır.
        //Lobi Steam Kimliği, requestLobbyList ile arama yaparak, bir arkadaşınıza katılarak veya davet yoluyla elde edilebilir.
        SteamMatchmaking.JoinLobby(_steamIDLobby );
        //Hangi kullanıcının lobide olduğunu yinelemek için
        SteamMatchmaking.GetNumLobbyMembers(_steamIDLobby);
        //Bu, GetNumLobbyMembers'a yapılan önceki çağrıda kullanılan lobinin aynısı OLMALIDIR!
        //0 ile GetNumLobbyMembers arasında bir dizin.
        SteamMatchmaking.GetLobbyMemberByIndex(_steamIDLobby,1);
    }
    
    //Lobiyle iletişim kurma
    
    public void UserFriendsLobbies()
    {
        //Bir kullanıcının arkadaşlarının yer aldığı bütün lobileri arkadaşlar API'ını kullanarak bulabilirsiniz:
        int cFriends = SteamFriends.GetFriendCount( EFriendFlags.k_EFriendFlagAll );
        for ( int i = 0; i < cFriends; i++ )
        {
            FriendGameInfo_t friendGameInfo;
            CSteamID steamIDFriend = SteamFriends.GetFriendByIndex( i, EFriendFlags.k_EFriendFlagAll );
            if ( SteamFriends.GetFriendGamePlayed( steamIDFriend, out friendGameInfo ) && friendGameInfo.m_steamIDLobby.IsValid() )
            {
                // friendGameInfo.m_steamIDLobby geçerli bir lobidir, buna katılabilir ya da RequestLobbyData() kullanarak üstverisini alabilirsiniz
            }
        }
    }
    
    //Lobi veri alma(Arkadaşının ise)
    public void RequestLobbyData(CSteamID steamIDLobby )
    {
        //Şu anda içinde olmadığınız bir lobinin tüm meta verilerini yeniler.
        //Üyesi olduğunuz lobiler için bunu asla yapmayacaksınız, o veriler her zaman güncel olacaktır.
        //Bunu, requestLobbyList'ten edindiğiniz veya arkadaşlarınız aracılığıyla erişilebilen lobileri yenilemek için kullanabilirsiniz.
        //Return: bool LobbyDataUpdate_t geri çağrısını tetikler. istek sunucuya başarıyla gönderildiyse true. Steam ile bağlantı kurulamadıysa veya steamIDLobby geçersizse false.
        //Belirtilen lobi yoksa LobbyDataUpdate_t::m_bSuccess false olarak ayarlanacaktır.
        SteamMatchmaking.RequestLobbyData(steamIDLobby);
    }

    //Lobi Sahibi Yapabileceği şeyler
    public void SetLobbyData(CSteamID steamIDLobby)
    {
        //steamIDLobby CSteamID Meta verinin ayarlanacağı lobinin Steam Kimliği.
        //pchKey const char * Verilerin ayarlanacağı anahtar. Bu, k_nMaxLobbyKeyLength'ten daha uzun olamaz.
        //pchValue const char * Ayarlanacak değer. Bu k_cubChatMetadataMax'tan daha uzun olamaz.
        SteamMatchmaking.SetLobbyData(steamIDLobby,"pchKey","pchValue");
        //Lobi meta verilerinde bir anahtar/değer çifti ayarlar. Bu, lobi adını, mevcut haritayı, oyun modunu vb. ayarlamak için kullanılabilir.
        //Bu yalnızca lobinin sahibi tarafından ayarlanabilir. Lobi üyeleri bunun yerine SetLobbyMemberData'yı kullanmalıdır.
        //Lobideki her kullanıcı, LobbyDataUpdate_t geri araması aracılığıyla lobi veri değişikliğine ilişkin bildirim alacaktır ve katılan tüm yeni kullanıcılar, mevcut tüm verileri alacaktır.
        //Bu, yalnızca değişiklik olması durumunda verileri gönderir. Verilerin gönderilmesinden önce hafif bir gecikme olur,
        //böylece ihtiyaç duyduğunuz tüm verileri ayarlamak için bunu tekrar tekrar arayabilirsiniz ve veriler otomatik olarak toplanıp son sıralı çağrıdan sonra gönderilir.
        //Return: bool veriler başarıyla ayarlandıysa true. steamIDLobby geçersizse veya anahtar/değer çok uzunsa false.
    }
    
    public void GetLobbyData(CSteamID steamIDLobby)
    {
        //steamIDLobby CSteamID Meta verilerin alınacağı lobinin Steam Kimliği.
        //pchKey const char * Değerinin alınacağı anahtar
        SteamMatchmaking.GetLobbyData(steamIDLobby,"pchKey");
        //Belirtilen anahtarla ilişkili meta verileri belirtilen lobiden alır.
        //NOT: Bu, yalnızca LobbyMatchList_t'den lobilerin bir listesini aldıktan, verileri requestLobbyData ile aldıktan sonra veya
        //bir lobiye katıldıktan sonra müşterinin bildiği lobilerden meta veriler alabilir.
        //Döndürür: const char * Bu anahtar için herhangi bir değer ayarlanmamışsa veya steamIDLobby geçersizse boş bir dize ("") döndürür.
    }

    public void DeleteLobbyData(CSteamID steamIDLobby)
    {
        SteamMatchmaking.DeleteLobbyData(steamIDLobby,"pchKey");
        //Lobiden bir meta veri anahtarını kaldırır.
        //Bu sadece lobinin sahibi tarafından yapılabilir
        //Bu, yalnızca anahtarın mevcut olması durumunda verileri gönderir. Verilerin gönderilmesinden önce hafif bir gecikme olur,
        //böylece ihtiyaç duyduğunuz tüm verileri ayarlamak için bunu tekrar tekrar arayabilirsiniz ve veriler otomatik olarak toplanıp son sıralı çağrıdan sonra gönderilir.
        //Return: bool anahtar/değer başarıyla silindiyse true; aksi halde, steamIDLobby veya pchKey geçersizse false.
    }
    
    //lobi sahibi düzenleme yaparken hata ayıklama 
    public void GetLobbyDataCount(CSteamID steamIDLobby)
    {
        SteamMatchmaking.GetLobbyDataCount(steamIDLobby);
        //Bu, yalnızca LobbyMatchList_t'den lobilerin bir listesini aldıktan,
        //verileri requestLobbyData ile aldıktan sonra veya bir lobiye katıldıktan sonra müşterinin bildiği lobilerden meta veriler alabilir.
        //Bu yineleme için kullanılır, bunu çağırdıktan sonra GetLobbyDataByIndex her meta veri parçasının anahtar/değer çiftini almak için kullanılabilir.
        //Bu genellikle yalnızca hata ayıklama amacıyla kullanılmalıdır.
        //Döndürür: int steamIDLobby geçersizse 0 değerini döndürür.
    }
    //Example
    void ListLobbyData( CSteamID lobbyID )
    {
        int nData = SteamMatchmaking.GetLobbyDataCount( lobbyID );
        char key;
        char value;
        for( int i = 0; i < nData; ++i )
        {
            //bool bSuccess = SteamMatchmaking.GetLobbyDataByIndex( lobbyID, i, key, k_nMaxLobbyKeyLength, value, k_cubChatMetadataMax );
            if (true )//(bSuccess )
            {
                //print( "Lobby Data %d, Key: \"%s\" - Value: \"%s\"\n", i, key, value );
            }
        }
    }

    public void GetLobbyDataByIndex(CSteamID steamIDLobby,string phckeys,string phcvalues)
    {
        //iLobbyData int 0 ile GetLobbyDataCount arasında bir dizin.
        //cchValueBufferSize int pchValue için ayrılan arabelleğin boyutu. Bu genellikle k_cubChatMetadataMax olmalıdır.
        SteamMatchmaking.GetLobbyDataByIndex(steamIDLobby,1,out phckeys,1,out phcvalues,1);
        //Dizine göre bir lobi meta veri anahtar/değer çifti alır.
        //NOT: Bunu çağırmadan önce GetLobbyDataCount'u aramalısınız.
        //Return: bool başarı üzerine doğru; aksi halde, steamIDLobby veya iLobbyData geçersizse false.
    }
    
    //Lobideki üyelerin güncellemeleri alabilecekleri kendi meta verilerini ayarlamasına da imkân tanıma
    public void GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, char pchKey )
    {
        //steamIDLobby CSteamID Diğer oyuncunun bulunduğu lobinin Steam ID'si.
        //steamIDUser CSteamID Meta verinin alınacağı oyuncunun Steam Kimliği.
        //pchKey const char * Değerinin alınacağı anahtar.
        SteamMatchmaking.GetLobbyMemberData(steamIDLobby,steamIDUser,"A");
        //Belirtilen lobideki başka bir oynatıcıdan kullanıcı başına meta verileri alır.
        //Bu yalnızca şu anda bulunduğunuz lobilerdeki üyelerden sorgulanabilir.
        //Döndürür: const char * steamIDLobby geçersizse veya steamIDUser lobide değilse NULL değerini döndürür.
        //Oynatıcı için pchKey ayarlanmamışsa boş bir dize ("") döndürür. Ayrıca Bakınız: SetLobbyMemberData
    }

    public void SetLobbyMemberData(CSteamID steamIDLobby, char pchKey)
    {
        //Yerel kullanıcı için kullanıcı başına meta verileri ayarlar. Lobideki her kullanıcı,
        //LobbyDataUpdate_t geri araması aracılığıyla lobi veri değişikliğine ilişkin bildirim alacaktır ve katılan tüm yeni kullanıcılar, mevcut tüm verileri alacaktır.
        SteamMatchmaking.SetLobbyMemberData(steamIDLobby,"A","B");
        
    }
}
