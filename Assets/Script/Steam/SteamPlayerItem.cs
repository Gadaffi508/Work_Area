using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SteamPlayerItem : MonoBehaviour
{
    //Eklenecek Oyuncunun Özellikleri
    public string PlayerName;
    public int ConnectionId;
    public ulong PlayerSteamID;
    private bool AvatarReceived;

    //Eklenmiş olan oyuncunun görüncek özellikleri
    public Text PlayerNameText;
    public RawImage PlayerIcon;

    //Steam Avatar geri araması yani veri çekme
    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start()
    {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    //Oyuncu özelliklerini getirip girme
    public void SetPlayerValues()
    {
        PlayerNameText.text = PlayerName;
        //Resim gelipo gelmeme kontrol
        if (!AvatarReceived) GetPlayerIcon();
    }

    void GetPlayerIcon()
    {
        //Arkadaşlarımızın fotosunu yüklmeme
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if(ImageID == -1) return;
        PlayerIcon.texture = GetSteamImageAsTexture(ImageID);
    }
    
    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        //Steam kimliği kontrol etme
        if (callback.m_steamID.m_SteamID == PlayerSteamID)
        {
            PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        //Another Player
        else return;
    }

    private Texture2D GetSteamImageAsTexture(int Image)
    {
        //Boş resim oluşturma
        Texture2D texture = null;
        //Oyuncu resim ve boyut getirme
        bool isValid = SteamUtils.GetImageSize(Image, out uint width,out uint height);
        //resim varmı kontrol etme
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];
            //renk getirme
            isValid = SteamUtils.GetImageRGBA(Image,image,(int)(width * height * 4));
            if (isValid)
            {
                //oluşan texture doldurma
                texture = new Texture2D((int)width,(int)height,TextureFormat.RG32,false,true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        //resmi döndürme
        AvatarReceived = true;
        return texture;
    }
}























