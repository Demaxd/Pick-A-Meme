using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pool : MonoBehaviour
{

    private Card _card;
    [SerializeField] private int _capacity;

    private List<Card> _pool = new List<Card>();

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < _capacity; i++)
        {
            Card spawned = Instantiate(_card, transform);
            spawned.gameObject.SetActive(false);
            _pool.Add(spawned);
        }
    }

    //private bool TryGetCard(out GameObject result)
    //{
    //    //result = _pool.FirstOrDefault(p => p);

    //        //return result != null;
    //}

}
