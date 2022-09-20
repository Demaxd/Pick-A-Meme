using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Mirror;

public class TurnManager : NetworkBehaviour
{
    [SerializeField] private Field _field;

    private int _turningPlayer;
    public static event UnityAction<int, int> TurnChanged;

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameManager.ServerGameStarted += OnGameStart;
        _field.ServerPlayerTurned += ChangeTurn;
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        _field.ServerPlayerTurned -= ChangeTurn;
        GameManager.ServerGameStarted -= OnGameStart;
    }

    [Server]
    private void OnGameStart()
    {
        print("game started");
        _turningPlayer = 0;
        GameManager.PlayersGO[_turningPlayer].SetTurning(true);
        TurnChanged?.Invoke(0, _turningPlayer);
    }

    [Server]
    private void ChangeTurn(int turnedPlayer)
    {
        var players = GameManager.PlayersGO;
        if (turnedPlayer == players.Count - 1) _turningPlayer = 0;
        else _turningPlayer++;

        players[turnedPlayer].SetTurning(false);
        players[_turningPlayer].SetTurning(true);
        TurnChanged?.Invoke(turnedPlayer, _turningPlayer);
    }
}
