using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkedHandAnimationController : NetworkBehaviour
{
    [SerializeField] private InputActionProperty pinchAnimation;
    [SerializeField] private InputActionProperty grabAnimation;

    [SerializeField] private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            pinchAnimation.action.started += v =>   handAnimator.SetFloat("Pinch", v.ReadValue<float>());
            pinchAnimation.action.performed += v => handAnimator.SetFloat("Pinch", v.ReadValue<float>());
            pinchAnimation.action.canceled += v =>  handAnimator.SetFloat("Pinch", 0);

            grabAnimation.action.started += v =>    handAnimator.SetFloat("Grab", v.ReadValue<float>());
            grabAnimation.action.performed += v =>  handAnimator.SetFloat("Grab", v.ReadValue<float>());
            grabAnimation.action.canceled += v =>   handAnimator.SetFloat("Grab", 0);    
        }
    }
}
