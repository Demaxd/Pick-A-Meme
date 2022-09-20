using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Events;

public class Card : NetworkBehaviour
{
    [SerializeField] private Vector3 _spawnPoint = new Vector3(1200, -300, 0);

    private Image _image;
    private int _siblingIndex;

    public int DeckIndex;
    public event UnityAction<Player> OwnerAdded;


    private void Start()
    {
        _image = GetComponent<Image>();
    }

    
    public void Throw(Field field)
    {

        transform.SetAsLastSibling();
        transform.SetParent(field.transform, false);
    }
    public void Return()
    {
        _image.raycastTarget = true;
        transform.SetSiblingIndex(_siblingIndex);
    }

    public void OnOwnerAdded(Player player)
    {
        OwnerAdded?.Invoke(player);
        _siblingIndex = transform.GetSiblingIndex();
    }

}
