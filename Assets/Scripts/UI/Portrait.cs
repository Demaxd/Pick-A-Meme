using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Portrait : MonoBehaviour
{
    protected bool isTurning;

    protected Image Avatar;
    protected Text _nameText;
    protected int _connectionID;
    public bool IsTurning => isTurning;


    protected virtual void Awake()
    {
        Avatar = GetComponent<Image>();
        _nameText = GetComponentInChildren<Text>();
    }

    public virtual void Initialize(PlayerInfo playerInfo)
    {
        isTurning = false;
        _connectionID = playerInfo.ConnectionID;
        _nameText.text = playerInfo.Name;
        gameObject.name = playerInfo.Name;
    }
    public virtual void Highlight(bool isTurning) { this.isTurning = isTurning; }

}
