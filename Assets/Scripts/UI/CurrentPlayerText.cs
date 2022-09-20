using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerText : MonoBehaviour
{
    [SerializeField] private Field _field;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        //_field.PlayerTurned += OnPlayedTurned;
    }

    private void OnDisable()
    {
        //_field.PlayerTurned -= OnPlayedTurned;
    }

    private void OnPlayedTurned(Player player, Card card)
    {
        //_text.text = $"Последний рар сходил игрок {player.Number} и кинул {card.name}!";
    }
}
