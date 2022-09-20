using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardSizer : MonoBehaviour
{ 
    [SerializeField] private Vector2 _handSize;
    [SerializeField] private Vector2 _onDragSize;
    [SerializeField] private float _settlingRate = 1f;
 
    private RectTransform _rectTransform;
    private Vector2 _targetSize;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _targetSize = _rectTransform.sizeDelta;
    }

    private void Update()
    {
        MoveToSize(_targetSize);
    }

    private void OnMouseDown()
    {
        _targetSize = _onDragSize;
    }
    private void OnMouseUp()
    {
        _targetSize = _handSize;
    }

    public void MoveToSize(Vector2 size)
    {
        _rectTransform.sizeDelta = Vector2.Lerp(_rectTransform.sizeDelta, size, _settlingRate * Time.deltaTime);
    }
}
