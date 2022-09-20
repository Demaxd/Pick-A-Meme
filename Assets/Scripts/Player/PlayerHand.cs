using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PlaceState
{
    Free,
    Taken
}
public class PlayerHand : MonoBehaviour
{
    [SerializeField] private CardDealer _dealer;
    [SerializeField] private List<GameObject> _cardsPlaces = new List<GameObject>();
    [HideInInspector] public int LastTaken;

    private List<Card> _activeCards = new List<Card>();
    private HorizontalLayoutGroup _horizontalLayoutGroup;

    private void Awake()
    {
        LastTaken = 0;
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        _horizontalLayoutGroup.enabled = false;
    }

    public List<GameObject> CardsPlaces => _cardsPlaces;
    
    public void SetCard(Card card, int placeNumber)
    {
        _activeCards.Add(card);
        if (placeNumber >= 4)
        {
            _horizontalLayoutGroup.enabled = true;

            foreach (var item in _activeCards)
            {
                MakeInteractive(item);
            }
        }
        
    }

    private void MakeInteractive(Card card)
    {
        card.GetComponent<CardDrag>().enabled = true;
        card.GetComponent<CardSizer>().enabled = true;
    }
}
