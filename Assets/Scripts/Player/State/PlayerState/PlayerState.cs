using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public abstract void OnCardThrow(Player player, Card card);

}
