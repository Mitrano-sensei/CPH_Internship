using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform root;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public Renderer[] meshToDisable;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var mesh in meshToDisable)
            {
                mesh.enabled = false;
            }
        }
    }
    
    public void Update()
    {
        if (IsOwner)
        {
            UpdateTransforms();
        }
        
    }

    private void UpdateTransforms()
    {
        if (!root || !head || !leftHand || !rightHand)
        {
            Debug.LogError("Missing references");
            return;
        }
        
        if (root.position.y < -10)
        {
            Debug.LogError("Player fell off the map");
            return;
        }

        var vrRigReferences = VRRigReferences.Instance;
        if (vrRigReferences == null)
        {
            Debug.LogError("Missing VRRigReferences");
            return;
        }
        
        root.position = vrRigReferences.Root.position;
        root.rotation = vrRigReferences.Root.rotation;
        
        head.position = vrRigReferences.Head.position;
        head.rotation = vrRigReferences.Head.rotation;
        
        leftHand.position = vrRigReferences.LeftHand.position;
        leftHand.rotation = vrRigReferences.LeftHand.rotation;
        
        rightHand.position = vrRigReferences.RightHand.position;
        rightHand.rotation = vrRigReferences.RightHand.rotation;
    }
}
