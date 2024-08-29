using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class NetworkConnection : MonoBehaviour
{
    [SerializeField] private int maxConnections = 2;
    [SerializeField] private UnityTransport transport;
    private Lobby _currentLobby;

    /*
    void Update()
    {
        // DEBUG : If C is pressed, create a client, if H is pressed create a host
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateClient();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            CreateHost();
        }
    }
    */

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        CreateOrJoin();
    }

    /**
     * Create a host if no lobbies are found, otherwise create a client,
     * Could be replaced with a Lobby list system in the future
     */
    private async void CreateOrJoin()
    {
        var joinCode = await FindLobbyJoinCode();

        if (joinCode != null)
            CreateClient(joinCode);
        else
            CreateHost();
    }
    
    private async Task<string> FindLobbyJoinCode()
    {
        var lobbies = await Lobbies.Instance.QueryLobbiesAsync();
        foreach (var lobby in lobbies.Results)
        {
            lobby.Data.TryGetValue("JOIN_CODE", out var joinCode);
            if (joinCode != null)
            {
                return joinCode.Value;
            }
        }

        return null;
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

    private async void CreateClient(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        
        transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
        
        Debug.Log("Joined lobby with join code: " + joinCode);
        NetworkManager.Singleton.StartClient();
    }
    
}
