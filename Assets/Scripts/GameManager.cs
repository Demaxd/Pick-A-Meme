using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mirror;
using System;

public class GameManager : NetworkBehaviour
{
    [Header("Game Settings")]
    [Range(1, 10)]
    [SerializeField] private int _playersToStart = 2;

    [Header("References")]
    [SerializeField] private Field _field;
    [SerializeField] private CardDealer _dealer;

    private readonly SyncDictionary<int, PlayerInfo> _playersInfo = new SyncDictionary<int, PlayerInfo>();
    private static Dictionary<int, Player> _playersGO = new Dictionary<int, Player>();
    public static Dictionary<int, Player> PlayersGO => _playersGO;
    public SyncDictionary<int, PlayerInfo> PlayersInfo => _playersInfo;

    public static event UnityAction ServerGameStarted;
    public static event UnityAction<PlayerInfo> ServerPlayerJoined;
    public static event UnityAction<PlayerInfo> ServerPlayerLeaved;

    #region Unity/Mirror Callbacks

    #endregion

    [Server]
    public void ServerAddPlayer(Player player, PlayerInfo playerInfo)
    {
        _playersGO.Add(playerInfo.ConnectionID, player);
        _playersInfo.Add(playerInfo.ConnectionID, playerInfo);

        ServerPlayerJoined?.Invoke(playerInfo);
        if (_playersInfo.Count == _playersToStart) ServerGameStarted?.Invoke();
    }
    public void ServerRemovePlayer(Player player)
    {
        int disconnectedID = player.connectionID;
        _playersGO.Remove(disconnectedID);
        ServerPlayerLeaved?.Invoke(PlayersInfo[disconnectedID]);
    }
}
