using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Mirror;

public class Field : NetworkBehaviour
{
    [Header("Templates")]
    [SerializeField] private Card _card;

    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CardDealer _dealer;


    private HorizontalLayoutGroup _horizontalLayoutGroup;
    private SyncList<int> _cardIndexes = new SyncList<int>();
    private Dictionary<int, Card> _cards = new Dictionary<int, Card>();
    

    public event UnityAction<int> ServerPlayerTurned;

    private void Awake()
    {
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        _horizontalLayoutGroup.enabled = false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //_gameManager.ServerPlayerJoined += ReloadCards;
    }

    private void DropCard(int cardNumberInDeck)
    {
        Card card = Instantiate(_card);
        card.transform.GetChild(0).GetComponent<Image>().sprite = _dealer.Pictures[cardNumberInDeck];
        card.GetComponent<CardDrag>().enabled = false;
        card.Throw(this);
        
    }

    [Command(requiresAuthority = false)]
    public void CmdDropCard(int playerID, int cardNumberInDeck)
    {
        ServerPlayerTurned?.Invoke(playerID);
        _cardIndexes.Add(cardNumberInDeck);
        RpcDropCard(playerID, cardNumberInDeck);
    }

    [ClientRpc]
    private void RpcDropCard(int playerID, int cardNumberInDeck) => DropCard(cardNumberInDeck);


}
