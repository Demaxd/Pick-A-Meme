using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{   [Header("Templates")]
    [SerializeField] private Card _card;

    [SyncVar] private bool _canTurn = false;
    [SyncVar] public int connectionID;

    private Field _field;
    private PlayerHand _hand;
    private HorizontalLayoutGroup _cardsHolder;
    private CardDealer _dealer;
    private CardTween _cardTween;

    public bool CanTurn => _canTurn;
    private Dictionary<int, Card> _cards = new Dictionary<int, Card>();// Key is number of card in CardDealer's pictures
    public PlayerHand Hand => _hand;

    #region Callbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
        _canTurn = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        InitializeObjects();
    }

    #endregion
    private void InitializeObjects()
    {
        _field = FindObjectOfType<Field>();
        _hand = FindObjectOfType<PlayerHand>();
        _cardsHolder = _hand.GetComponent<HorizontalLayoutGroup>();
        //_cardsHolder.enabled = false;
        _dealer = FindObjectOfType<CardDealer>();
        _cardTween = FindObjectOfType<CardTween>();
    }


    #region Draw Card
    [TargetRpc]
    public void AddCard(NetworkConnection conn, int index)
    {
        Card newCard = ShowCard(index);
        _cards.Add(index, newCard);
    }


    [Client]
    private Card ShowCard(int cardIndex)
    {
        var spawnPoint = _dealer.CardSpawnPoint;

        Card card = Instantiate(_card, spawnPoint.position, Quaternion.identity, spawnPoint);
        card.DeckIndex = cardIndex;
        card.transform.GetChild(0).GetComponent<Image>().sprite = _dealer.Pictures[cardIndex];
        card.transform.SetParent(_hand.transform);
        _cardTween.Draw(card);
        card.OnOwnerAdded(this);

        return card;
    }

    #endregion


    #region Turns

    [Server]
    public void SetTurning(bool isTurning)
    {
        _canTurn = isTurning;
        print(_canTurn);
    }


    public void Turn(Card card)
    {
        _field.CmdDropCard(connectionID, card.DeckIndex);
        _cards.Remove(card.DeckIndex);
        Destroy(card.gameObject);
        _cardsHolder.enabled = true;
    }
    public void ReturnCardToHand(Card card)
    {
        card.Return();
        _cardsHolder.enabled = true;
    }
    #endregion
}
public struct PlayerInfo : NetworkMessage
{
    public int ConnectionID;
    public string Name;
}


