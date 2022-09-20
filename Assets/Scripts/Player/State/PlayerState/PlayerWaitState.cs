using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitState : PlayerState
{
    public override void OnCardThrow(Player player, Card card)
    {
        //player.ReturnCardToHand(card);
    }
}
