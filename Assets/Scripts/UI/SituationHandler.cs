using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SituationHandler : NetworkBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextAsset _textFile;

    [SyncVar(hook = nameof(UpdateCurrent))] private string _currentText = "";

    private Text _uiText;
    private List<string> _situations;

    private void Awake()
    {
        _uiText = GetComponentInChildren<Text>();
        _situations = _textFile.text.Split('\n').ToList<string>();

        GameManager.ServerGameStarted += ServerChooseString;
    }

    [Server]
    private void ServerChooseString()
    { 
        int choosed = Random.Range(0, +_situations.Count);
        _currentText = _situations[choosed] + ":";
        _situations.RemoveAt(choosed);
    }
 

    private void UpdateCurrent(string oldValue, string newValue)
    {
        _uiText.text = newValue;
    }




}
