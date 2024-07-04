using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamPlayerGetScoreData : MonoBehaviour
{
    public string playerName;

    public int playerMS;

    [SerializeField] private Text playerNameText, playerMSText;

    public void ShowScoreBoard()
    {
        playerNameText.text = playerName;

        playerMSText.text = playerMS + " MS";
    }
}
