using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayBehaviour : MonoBehaviour
{
    private readonly BehaviourTree.BehaviourTree _behaviour = new();
    private Animator _animator;
    
    private static readonly int Dance = Animator.StringToHash("Dance");
    private static readonly int Hello = Animator.StringToHash("Hello");
    private static readonly int Idle = Animator.StringToHash("Idle");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        InitTestBehaviour();
    }

    // Update is called once per frame
    void Update()
    {
        _behaviour.Process();
    }
    
    private void InitTestBehaviour()
    {
        
        
    }

    private void DanceMove()
    {
        _animator.SetTrigger(Dance);
    }

    private void SayHello()
    {
        _animator.SetTrigger(Hello);
    }

    private void GoIdle()
    {
        _animator.SetTrigger(Idle);
    }
}
