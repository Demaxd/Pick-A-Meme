using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class Notifier : NetworkBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private Text _messageText;
    private Animation _animation;

    private void Awake()
    {
        _messageText = GetComponent<Text>();
        _animation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        GameManager.ServerPlayerJoined += ShowNewPlayer;
    }

    private void OnDisable()
    {
        GameManager.ServerPlayerJoined -= ShowNewPlayer;
    }


    [ClientRpc]
    private void ShowNewPlayer(PlayerInfo playerInfo)
    {
        _messageText.text = $"{playerInfo.Name} Подключился!";
        _animation.Play();
    }


}
