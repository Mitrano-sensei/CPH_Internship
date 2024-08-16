using System;
using BehaviourTree;
using UnityEngine;

[Serializable]
public class BehaviourDefinition
{
    [SerializeReference, SubclassSelector] public BehaviourNode node;
}

[Serializable]
public abstract class BehaviourNode
{
    [Header("Basic Info")]
    public string nodeName;
    
    public abstract Node GetNode();
}

#region BehaviourNodes
[Serializable]
public class WalkToNode : BehaviourNode
{
    [Header("Requirements")]
    [SerializeField] public DoorwayNavMeshAgent locomotionSystem;
    [SerializeField] public Transform moveToTarget;
    
    public override Node GetNode()
    {
        var sequence = new Sequence("WalkToNode");
        
        // Move
        var move = new Leaf(new ActionStrategy(() =>
        {
            locomotionSystem.MoveTo(moveToTarget.position);
        }));
        sequence.AddChild(move);
        
        
        // Wait for end of movement
        var waitForEndOfMovement = new UntilSuccess("Wait For End Of Movement");
        waitForEndOfMovement.AddChild(new ConditionLeaf(() => !locomotionSystem.IsMoving));
        sequence.AddChild(waitForEndOfMovement);

        return sequence;
    }
}

[Serializable]
public class PlayAnimationNode : BehaviourNode
{
    [Header("Requirements")]
    [SerializeField] public Animator animator;
    [SerializeField] public string animationName;
    [SerializeField] public string triggerName;
    
    public override Node GetNode()
    {
        var sequence = new Sequence("PlayAnimationNode");
        
        // Play Animation
        var playAnimation = new Leaf(new ActionStrategy(() =>
        {
            animator.SetTrigger(triggerName);
        }));
        sequence.AddChild(playAnimation);

        var waitALittle = new WaitLeaf(1f);
        sequence.AddChild(waitALittle);
        
        // Wait for end of animation
        var waitForEndOfAnimation = new UntilSuccess("Wait For End Of Animation");
        waitForEndOfAnimation.AddChild(new ConditionLeaf(IsAnimationFinished));
        sequence.AddChild(waitForEndOfAnimation);

        return sequence;
    }
    
    /**
     * Returns true if the dance animation is finished
     */
    private bool IsAnimationFinished()
    {
        var baseAnimationName = "Base."+animationName;
        
        var isAnimatedOrTransition = animator.GetCurrentAnimatorStateInfo(0).IsName(baseAnimationName) || animator.IsInTransition(0);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(baseAnimationName))
        {
            Debug.LogError("Flag 1 : " + baseAnimationName);
        }
        if (!animator.IsInTransition(0))
        {
            Debug.LogError("Flag 2");
        }
        return !isAnimatedOrTransition;
    }
}

[Serializable]
public class DebugNode : BehaviourNode
{
    public override Node GetNode()
    {
        return new DebugLeaf(nodeName);
    }
}

[Serializable]
public class WaitNode : BehaviourNode
{
    [Header("Requirements")]
    [SerializeField] public float waitTime;
    
    public override Node GetNode()
    {
        return new WaitLeaf(waitTime);
    }
}

[Serializable]
public class WaitForMovementNode : BehaviourNode
{
    [Header("Requirements")] 
    [SerializeField] public Collider triggerZone;
    [SerializeField] public Transform player;
    
    public override Node GetNode()
    {
        return new LocationLeaf(triggerZone, player);
    }
}

[Serializable]
public class DialogNode : BehaviourNode
{
    [Header("Requirements")] 
    [SerializeField] public DialogSystem dialogSystem;
    [SerializeField] public DialogInfo dialog;
    
    public override Node GetNode()
    {
        return new DialogCompleteLeaf(dialogSystem, dialog);    
    }
}

[Serializable]
public class RotateToNode : BehaviourNode
{
    [Header("Requirements")] 
    [SerializeField] public DoorwayNavMeshAgent locomotionSystem;
    [SerializeField] public Transform target;
    
    public override Node GetNode()
    {
        var sequence = new Sequence("RotateToNode");
        
        // Rotate
        var rotate = new Leaf(new ActionStrategy(() =>
        {
            locomotionSystem.RotateTo(target);
        }));
        sequence.AddChild(rotate);
        
        // Wait for end of rotation
        var waitForEndOfRotation = new WaitLeaf(locomotionSystem.RotationTime);
        sequence.AddChild(waitForEndOfRotation);

        return sequence;
    }
}
#endregion