using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
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
        var mainSequence = new Sequence("Main Sequence");
        
        var doDance = new Leaf(new ActionStrategy(DanceMove));
        var doHello = new Leaf(new ActionStrategy(SayHello));
        var doIdle = new Leaf(new ActionStrategy(GoIdle));
        var waitTwoSec = new WaitLeaf(2f);
        var waitForDance = new UntilSuccess("Wait For Dance");
        waitForDance.AddChild(new ConditionLeaf(IsDanceFinished));
        
        mainSequence.AddChild(new DebugLeaf("Starting"));
        mainSequence.AddChild(doDance);
        mainSequence.AddChild(waitForDance);
        mainSequence.AddChild(new DebugLeaf("Dance Finished"));
        // mainSequence.AddChild(doIdle);
        mainSequence.AddChild(waitTwoSec);
        mainSequence.AddChild(doHello);
        mainSequence.AddChild(new DebugLeaf("Ending"));
        
        _behaviour.AddChild(mainSequence);
    }

    private bool IsDanceFinished()
    {
        var isDancingOrTransition = _animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Dance") || _animator.IsInTransition(0);
        
        return !isDancingOrTransition;
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
