using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public enum PortraitType
{
    Player,
    Enemy
}
public class PortraitsDisplayer : NetworkBehaviour
{
    [Header("Templates")]
    [SerializeField] private Portrait _playerPortrait;
    [SerializeField] private EnemyPortrait _enemyPortrait;

    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Field _field;

    [Header("Options")]
    [SerializeField] private float _secondsToWaitBeforeCallback = 0;

    private Dictionary<int, Portrait> _Portraits = new Dictionary<int, Portrait>();
    private Player _localPlayer;


    #region Mirror Callbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
        GameManager.ServerPlayerJoined += OnPlayerConnected;
        GameManager.ServerPlayerLeaved += RemoveEnemyPortrait;
        TurnManager.TurnChanged += SwitchHighlighted;
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager.ServerPlayerJoined -= OnPlayerConnected;
        GameManager.ServerPlayerLeaved -= RemoveEnemyPortrait;
        TurnManager.TurnChanged -= SwitchHighlighted;
    }

    #endregion


    [Client]
    private void InitializePlayers(SyncDictionary<int, PlayerInfo> players)
    {
        _localPlayer = NetworkClient.localPlayer.GetComponent<Player>();
        foreach (var player in players)
        {
            if (player.Key == _localPlayer.connectionID) AddPortrait(PortraitType.Player, player.Value);
            else AddPortrait(PortraitType.Enemy, player.Value);
        }
    }

    [ClientRpc]
    private void OnPlayerConnected(PlayerInfo playerInfo) => StartCoroutine(OnPlayerConnectedRoutine(playerInfo));


    [Client]
    private IEnumerator OnPlayerConnectedRoutine(PlayerInfo playerInfo)
    {
        var players = _gameManager.PlayersInfo;
        yield return null;

        if (_Portraits.Count == 0) InitializePlayers(players);
        else //if Count > 0 own portrait is initialized
        {
            AddPortrait(PortraitType.Enemy, playerInfo);
        }

        //_Portraits[0].SetCurrent(true); //Game starts with player with connID = 0;
    }

    [Client]
    private void AddPortrait(PortraitType portraitType, PlayerInfo playerInfo)
    {
        Portrait portrait = null;
        switch (portraitType)
        {
            case PortraitType.Enemy:
                portrait = Instantiate(_enemyPortrait, transform);
                break;
            case PortraitType.Player:
                portrait = Instantiate(_playerPortrait, transform);
                break;
        }

        portrait.Initialize(playerInfo);
        _Portraits.Add(playerInfo.ConnectionID, portrait);

        if (portrait.IsTurning) portrait.Highlight(true);
    }

    [ClientRpc]
    private void RemoveEnemyPortrait(PlayerInfo playerInfo)
    {

        Portrait portraitToRemove = _Portraits[playerInfo.ConnectionID];
        Destroy(portraitToRemove.gameObject);
        _Portraits.Remove(playerInfo.ConnectionID);
    }

    [ClientRpc]
    private void SwitchHighlighted(int turnedPlayer, int turningPlayer)
    {
        SetCurrent(turnedPlayer, false);
        SetCurrent(turningPlayer, true);
    }

    [Client]
    private void SetCurrent(int portraitIndex, bool isHighlighted)
    {
        if (_Portraits.TryGetValue(portraitIndex, out Portrait portrait))
            portrait.Highlight(isHighlighted);
    }
    




}
