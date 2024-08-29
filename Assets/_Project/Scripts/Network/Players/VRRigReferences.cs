using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class VRRigReferences : Singleton<VRRigReferences>
{
    [Header("References")] 
    [SerializeField] private Transform root;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public Transform Root => root;
    public Transform Head => head;
    public Transform LeftHand => leftHand;
    public Transform RightHand => rightHand;
}
