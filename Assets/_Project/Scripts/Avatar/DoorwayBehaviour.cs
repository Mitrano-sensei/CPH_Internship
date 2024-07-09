using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class DoorwayBehaviour : MonoBehaviour
{
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] private DialogInfo dialogInfo;
    
    private readonly BehaviourTree.BehaviourTree _behaviour = new();
    private Animator _animator;
    
    private static readonly int Dance = Animator.StringToHash("Dance");
    private static readonly int Hello = Animator.StringToHash("Hello");
    private static readonly int Idle = Animator.StringToHash("Idle");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        InitTestBehaviour2();
    }

    // Update is called once per frame
    void Update()
    {
        _behaviour.Process();
    }
    
    /**
     * Test Purpose
     */
    private void InitTestBehaviour()
    {
        var mainSequence = new Sequence("Main Sequence");
        
        var doDance = new Leaf(new ActionStrategy(DanceMove));
        var doHello = new Leaf(new ActionStrategy(SayHello));
        var waitTwoSec = new WaitLeaf(2f);
        var waitForDance = new UntilSuccess("Wait For Dance");

        var playDialog = new DialogCompleteLeaf(dialogSystem, dialogInfo, "Hello and Dance!");
        
        waitForDance.AddChild(new ConditionLeaf(IsDanceFinished));
        
        mainSequence.AddChild(new DebugLeaf("Starting"));
        mainSequence.AddChild(doHello);
        mainSequence.AddChild(playDialog);
        mainSequence.AddChild(doDance);
        mainSequence.AddChild(waitForDance);
        mainSequence.AddChild(new DebugLeaf("Dance Finished"));
        mainSequence.AddChild(waitTwoSec);
        mainSequence.AddChild(new DebugLeaf("Ending"));
        
        _behaviour.AddChild(mainSequence);
    }

    /**
     * Test Purpose
     */
    private void InitTestBehaviour2()
    {
        var doDance = new Leaf(new ActionStrategy(DanceMove));
        var doHello = new Leaf(new ActionStrategy(SayHello));

        var waitForDance = new UntilSuccess("Wait For Dance");
        waitForDance.AddChild(new ConditionLeaf(IsDanceFinished));
        
        var startDialog = new DialogStartLeaf(dialogSystem, dialogInfo, "Start Dialog");
        var nextDialog = new DialogNextLineLeaf(dialogSystem, "Next Dialog");
        var finishDialog = new DialogFinishLeaf(dialogSystem, "Dialog Finish");
        
        var mainSequence = new Sequence("Main Sequence");
        
        mainSequence.AddChild(new DebugLeaf("Starting"));
        mainSequence.AddChild(doHello);
        mainSequence.AddChild(startDialog);
        
        mainSequence.AddChild(doDance);
        mainSequence.AddChild(nextDialog);
        mainSequence.AddChild(waitForDance);
        
        mainSequence.AddChild(finishDialog);
        
        mainSequence.AddChild(new DebugLeaf("Ending"));
        
        _behaviour.AddChild(mainSequence);
    }

    
    /**
     * Returns true if the dance animation is finished
     */
    private bool IsDanceFinished()
    {
        var isDancingOrTransition = _animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Dance") || _animator.IsInTransition(0);
        
        return !isDancingOrTransition;
    }

    /**
     * Play the dance animation
     */
    private void DanceMove()
    {
        _animator.SetTrigger(Dance);
    }

    /**
     * Play the hello animation
     */
    private void SayHello()
    {
        _animator.SetTrigger(Hello);
    }
}
