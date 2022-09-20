using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkHUD : MonoBehaviour
{
    //[SerializeField] private Button _hostButton;
    //[SerializeField] private Button _clientButton;
    [SerializeField] private InputField _addressInput;

    private CustomNetworkManager _manager;

    private void Awake()
    {
        _manager = GetComponent<CustomNetworkManager>();
        _addressInput.onEndEdit.AddListener(SetAddress);
    }


    public void StartHost()
    {
        _manager.StartHost();
    }

    public void StartClient()
    {
        _manager.StartClient();
    }

    public void SetAddress(string address)
    {
        print(123);
        _manager.networkAddress = address;
    }
    

    

}
