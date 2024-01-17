using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamManagerLearn : MonoBehaviour
{
    private static SteamManagerLearn s_instance;
    private static SteamManagerLearn Instance 
    {
        get 
        {
            return s_instance ?? new GameObject("SteamManager").AddComponent<SteamManagerLearn>();
        }
    }
    
    private bool m_bInitialized;
    
    public static bool Initialized 
    {
        get 
        {
            return Instance.m_bInitialized;
        }
    }
    
    //Bir işlev temsilcisiyle SteamClient.SetWarningMessageHook()'u çağırarak belirli durumlarda Steam'den gelen uyarı mesajlarına müdahale edebiliriz.
    private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

    private void Awake() 
    {
        if (s_instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;

        DontDestroyOnLoad(gameObject);
        
        //Steamworks.NET'in doğru platform altında çalışmasını sağlar. Normal çalışma koşullarında Unity'de bu asla false değeri döndürmemelidir.
        if (!Packsize.Test()) 
        {
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        }

        //Steamworks yeniden dağıtılabilir ikili dosyalarının doğru sürüm olduğundan emin olmak için kontrol eder.
        //Bu özellikle Steamworks.NET'i yükselttiğinizde, özellikle de Steamworks.NET düzenleyici komut dosyalarını kullanmıyorsanız kullanışlıdır.
        //Steamworks.NET'i yanlış steam_api.dll dosyasıyla çalıştırmak muhtemelen sorunlara neden olacaktır. (Şu anda yalnızca steam_api[64].dll'yi kontrol ediyor)
        if (!DllCheck.Test()) 
        {
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        }
        
        //Bu, adı verilen ilk Steamworks işlevi olduğundan, steam_api.dll dosyasının yüklenebilmesini sağlamak için ideal konumdur.
        //Bu, DllNotFoundException'ı yakalamak için bu işlev çağrısının bir try..catch bloğuna sarılmasıyla gerçekleştirilir.
        try {
            if (SteamAPI.RestartAppIfNecessary((AppId_t)480)) 
            {
                Application.Quit();
                return;
            }
        }
        catch (System.DllNotFoundException e) 
        {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

            Application.Quit();
            return;
        }
        
        //SteamManager, herhangi bir Steamworks işlevini çağırmadan önce SteamAPI'nin başlatıldığından emin olmak için
        //diğer komut dosyalarından kullanabileceğiniz Başlatılmış özelliğini herkese açık olarak sunar.
        m_bInitialized = SteamAPI.Init();
        if (!m_bInitialized) 
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

            return;
        }
    }

    private void Update()
    {
        //Geri Arama ve CallResult sistemlerinin olayları göndermesi için SteamAPI.RunCallbacks sık sık çağrılmalıdır.
        //Çağrılar arasındaki süre ne kadar uzun olursa, Steam API'sinden etkinlik veya sonuçların alınması arasındaki potansiyel gecikme de o kadar fazla olur.
        //Time.timeScale'i 0'a ayarlayarak oyunu duraklatırsanız Update() işlevleri artık çalışmayacaktır.
        //SteamAPI.RunCallbacks'in oyununuz duraklatıldığında bile çalıştığından emin olmak için alternatiflere bakmak isteyeceksiniz. Ortak rutinler sizin için iyi bir seçenek olabilir.
        
        //Steam istemcisi geri aramalarını çalıştırın
        SteamAPI.RunCallbacks();
    }

    private static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText) 
    {
        Debug.LogWarning(pchDebugText);
    }

    private void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        
        //Bunu OnEnable'da çağırıyoruz, böylece Unity, komut dosyalarını yeniden derlerken olduğu gibi bir Montaj Yeniden Yüklemesi yaptıktan sonra yeniden oluşturulur
        //Steam'den uyarı mesajları almak için oyununuzu başlatma argümanlarında “-debug_steamapi” ile başlatmalısınız.
        if (m_SteamAPIWarningMessageHook == null) 
        {
            m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
        }
    }

    private void OnDestroy() 
    {
        if (s_instance != this)
        {
            return;
        }
        s_instance = null;
        
        //SteamManager'ın yapacağı son çağrı SteamAPI.Shutdown olacaktır; bu, SteamAPI'yi temizler ve Steam'in kapatmaya hazır olduğunuzu bilmesini sağlar.
        //OnDestroy, kapatıldığında çağrılan son şey olduğu için kullanılır.
        //SteamManager'ın kalıcı olması ve hiçbir zaman devre dışı bırakılmaması veya yok edilmemesi gerektiğinden, SteamAPI'yi kapatmak için OnDestroy'u kullanabiliriz.
        
        SteamAPI.Shutdown();
    }
}
