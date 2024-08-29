using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class NetworkGrab : NetworkBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Start()
    {
        
    }
}
