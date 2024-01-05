using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerController Player;

    public List<PlayerController> GamePlayer { get; } = new List<PlayerController> ();
}
