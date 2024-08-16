using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DebugNetwork : MonoBehaviour
{
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
    
    private void CreateClient()
    {
        Debug.Log("Creating Client");
        NetworkManager.Singleton.StartClient();
    }
    
    private void CreateHost()
    {
        Debug.Log("Creating Host");
        NetworkManager.Singleton.StartHost();
    }
}
