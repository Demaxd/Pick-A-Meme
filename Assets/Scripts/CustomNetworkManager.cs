using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager
{
    [Header("Custom Settings")]
    [Space]

    [SerializeField] private InputField _inputField;
    [SerializeField] private GameManager _gameManager;

    private static int _lastConnectionID = 0;
    public static event UnityAction<PlayerInfo> ServerPlayerDisconnected;

    #region Callbacks
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<PlayerInfo>(OnServerConnect);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        PlayerInfo playerMessage = new PlayerInfo
        {
            Name = _inputField.text
        };
        NetworkClient.Send(playerMessage);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        _gameManager.ServerRemovePlayer(conn.identity.GetComponent<Player>());
        NetworkServer.DestroyPlayerForConnection(conn);
    }

    [Server]
    private void OnServerConnect(NetworkConnection conn, PlayerInfo playerInfo)
    {
        GameObject playerObj = Instantiate(playerPrefab);
        playerObj.name = $"{playerInfo.Name}";

        Player player = playerObj.GetComponent<Player>();
        player.connectionID = _lastConnectionID++;

        playerInfo.ConnectionID = player.connectionID;

        NetworkServer.AddPlayerForConnection(conn, playerObj);
        _gameManager.ServerAddPlayer(player, playerInfo);
    }

    #endregion


}