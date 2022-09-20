using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Events;

[RequireComponent(typeof(Card))]
public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Player _player;
    private Card _card;
    private HorizontalLayoutGroup _HorLayoutGroup;
    private Camera _camera;
    private Image _image;
    private Vector3 _offset;


    private void Awake()
    {
        _card = GetComponent<Card>();
        _camera = Camera.main;
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _card.OwnerAdded += SetOwner;
    }

    private void OnDisable()
    {
        _card.OwnerAdded -= SetOwner;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _camera.ScreenToWorldPoint(eventData.position);
        _image.raycastTarget = false;
        _HorLayoutGroup.enabled = false;

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = _camera.ScreenToWorldPoint(eventData.position) + _offset;
        transform.position = newPosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        bool isField = target.TryGetComponent(out Field field);

        if (isField && _player.CanTurn)
        {
            _player.Turn(_card);
        }
        else
        {
            _player.ReturnCardToHand(_card);
        }

    }

    public void SetOwner(Player player)
    {
        _player = player;
        _HorLayoutGroup = _player.Hand.GetComponent<HorizontalLayoutGroup>();
    }



}
