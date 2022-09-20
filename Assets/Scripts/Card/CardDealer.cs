using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public struct CardData
{
    public Sprite CardSprite;
    public int DeckIndex;
    public int RemovedFrom;
 }

public class CardDealer : NetworkBehaviour
{
    [Header("Templates")]
    [SerializeField] private Card _card;

    [Header("References")]
    [SerializeField] private Transform _cardSpawnPoint;
    [SerializeField] private CardTween _cardTween;

    [Header("Settings")]
    [SerializeField] private List<Sprite> _availablePictures = new List<Sprite>();
    [SerializeField] private int _maxCardsToDraw = 5;
    [SerializeField] private float _delayBeforeDraws = 1f;

    private List<CardData> _deck = new List<CardData>();
    public List<Sprite> Pictures => _availablePictures;
    public int MaxCardsToDraw => _maxCardsToDraw;
    public Transform CardSpawnPoint => _cardSpawnPoint;

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeDeck();
        GameManager.ServerPlayerJoined += OnServerConnect;
    }

    [Server]
    private void InitializeDeck()
    {
        for (int i = 0; i < _availablePictures.Count; i++)
        {
            CardData card = new CardData
            {
                CardSprite = _availablePictures[i],
                DeckIndex = i,
            };
            _deck.Add(card);
        }
    }


    [Server]
    private void OnServerConnect(PlayerInfo playerInfo)
    {
        Player joinedPlayer = GameManager.PlayersGO[playerInfo.ConnectionID];
        StartCoroutine(ServerDrawCard(joinedPlayer.connectionToClient, _maxCardsToDraw));
    }



    [Server]
    private CardData GetRandomCard()
    {
        int index = Random.Range(0, _deck.Count);
        return new CardData
        {
            CardSprite = _deck[index].CardSprite,
            DeckIndex = _deck[index].DeckIndex,
            RemovedFrom = index
        };

    }

    [Server]
    private IEnumerator ServerDrawCard(NetworkConnection conn, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CardData cardData = GetRandomCard();
            int chosenCardNumber = cardData.DeckIndex;

            Player player = conn.identity.GetComponent<Player>();
            player.AddCard(conn, chosenCardNumber);

            _deck.RemoveAt(cardData.RemovedFrom);
            yield return new WaitForSeconds(_delayBeforeDraws);
        }

    }

}
