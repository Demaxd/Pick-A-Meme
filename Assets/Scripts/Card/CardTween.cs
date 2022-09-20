using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardTween : MonoBehaviour
{
    [SerializeField] private PlayerHand _hand;
    [SerializeField] private Field _field;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _showPoint;
    

    public void Draw(Card card)//used for draw in player's hand
    {

        Sequence DrawSeq = DOTween.Sequence();
        DrawSeq.Append(card.transform.DOMove(_showPoint.position, 1f));

        int placeIndex = _hand.LastTaken++;
        Vector3 placeToMove = _hand.CardsPlaces[placeIndex].transform.position;
        DrawSeq.Append(card.transform.DOMove(placeToMove,1.5f));
        DrawSeq.AppendCallback(()=> _hand.SetCard(card, placeIndex));
    }

    public void Drop(Card card)// used for drop on field
    {
        
    }


}
