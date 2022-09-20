using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyPortrait : Portrait
{
    [SerializeField] private float _normalSize = 100f;
    [SerializeField] private float _onTurnSize = 200f;

    private RectTransform _rectTransform;

    protected override void Awake()
    {
        base.Awake();
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public override void Highlight(bool isTurning)
    {
        base.Highlight(isTurning);

        float newSize;
        newSize = isTurning == true? _onTurnSize: _normalSize;
        _rectTransform.sizeDelta = new Vector2(newSize, newSize);
    }
}
