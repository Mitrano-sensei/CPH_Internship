using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkConnection : MonoBehaviour
{
    [SerializeField] private int maxConnections = 2;
    [SerializeField] private UnityTransport transport;
    private Lobby _currentLobby;

    void Update()
    {
        // If C is pressed, create a client, if H is pressed create a host
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateClient();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            CreateHost();
        }
    }

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //CreateOrJoin();
    }

    private async void CreateOrJoin()
    {
        try
        {
            _currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            var joinCode = _currentLobby.Data["JOIN_CODE"].Value;

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        
            transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
        
            NetworkManager.Singleton.StartClient();
        }
        catch
        {
            CreateHost();
        }
    }
    
    private async void CreateHost()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        
        transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        CreateLobbyOptions lobbyOptions = new()
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>()
        };
        DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, newJoinCode);
        lobbyOptions.Data.Add("JOIN_CODE", dataObject);

        _currentLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby Name", maxConnections, lobbyOptions);

        Debug.Log("Created lobby with join code: " + newJoinCode);
        NetworkManager.Singleton.StartHost();
    }
    
    private async void CreateClient()
    {
        _currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
        var joinCode = _currentLobby.Data["JOIN_CODE"].Value;

        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        
        transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
        
        Debug.Log("Joined lobby with join code: " + joinCode);
        NetworkManager.Singleton.StartClient();
    }
    
    
}
