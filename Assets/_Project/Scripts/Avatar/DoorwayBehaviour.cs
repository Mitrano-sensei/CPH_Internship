using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

public class DoorwayBehaviour : MonoBehaviour
{
    [Header("Dialog")]
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] private DialogInfo mainDialog;
    
    private Animator _animator;
    private static readonly int Dance = Animator.StringToHash("Dance");
    private static readonly int Hello = Animator.StringToHash("Hello");
    private static readonly int Idle = Animator.StringToHash("Idle");
    
    [Header("Locomotion")]
    [SerializeField] private DoorwayNavMeshAgent locomotionSystem;
    
    [Header("BehaviourBuilder")]
    [SerializeField] private List<BehaviourDefinition> behaviour;
    
    private readonly BehaviourTree.BehaviourTree _behaviourTree = new();

    void Start()
    {
        _animator = GetComponent<Animator>();

        InitBehaviourTree();
    }

    void Update()
    {
        _behaviourTree.Process();
    }

    private void InitBehaviourTree()
    {
        if (behaviour == null || behaviour.Count == 0)
        {
            Debug.LogError("No Behaviour Defined");
            return;
        }

        var mainSequence = new Sequence("Main Sequence");

        foreach (var node in behaviour.Select(n => n.node.GetNode()))
        {
            mainSequence.AddChild(node);
        }
        
        _behaviourTree.AddChild(mainSequence);
    }

    #region Old Tests
    /*
    private void InitTestBehaviour2()
    {
        var doDance = new Leaf(new ActionStrategy(DanceMove));
        var doHello = new Leaf(new ActionStrategy(SayHello));

        var waitForDance = new UntilSuccess("Wait For Dance");
        waitForDance.AddChild(new ConditionLeaf(IsDanceFinished));
        
        var startDialog = new DialogStartLeaf(dialogSystem, mainDialog, "Start Dialog");
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
        
        // TODO --> Here add the trigger zone and a second Dialog
        var waitForMovementInTables = new UntilSuccess("Wait For Movement towards table area");
        var isInTableArea = new LocationLeaf(tableArea, player, "Is In Table Area");
        waitForMovementInTables.AddChild(isInTableArea);
        
        var waitForEndOfMovement = new UntilSuccess("Wait For End Of Movement");
        waitForEndOfMovement.AddChild(new ConditionLeaf(() => !locomotionSystem.IsMoving));

        var tableDialog = new DialogCompleteLeaf(dialogSystem, secondDialog, "Second Dialog");
        mainSequence.AddChild(new DebugLeaf("Waiting for movement in tables"));
        mainSequence.AddChild(waitForMovementInTables);
        mainSequence.AddChild(new DebugLeaf("Movement in tables detected, Moving"));
        mainSequence.AddChild(tableDialog);
        mainSequence.AddChild(new Leaf(new ActionStrategy(() => locomotionSystem.MoveTo(firstTestTarget))));
        
        mainSequence.AddChild(new DebugLeaf("Waiting for end of movement"));
        mainSequence.AddChild(waitForEndOfMovement);
        
        mainSequence.AddChild(new DebugLeaf("Rotating"));
        mainSequence.AddChild(new Leaf(new ActionStrategy(() => locomotionSystem.RotateTo(player.transform))));
        mainSequence.AddChild(waitForEndOfMovement);
        
        mainSequence.AddChild(new DebugLeaf("Ending"));
        
        _behaviourTree.AddChild(mainSequence);
    }
    */
    
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
    #endregion
}
