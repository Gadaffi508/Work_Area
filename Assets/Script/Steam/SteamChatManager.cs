using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamChatManager : MonoBehaviour
{
    public InputField chatMessage;

    public GameObject text;

    public Transform content;

    public void EnterText()
    {
        if(text.GetComponent<Text>().text == "") return;
        
        Instantiate(text,content);
        
        text.GetComponent<Text>().text = chatMessage.text;
    }
}
