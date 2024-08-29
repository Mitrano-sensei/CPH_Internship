using System;
using BehaviourTree;
using UnityEngine;

/**
 * Represents a behaviour definition that can be used to create a behaviour tree
 */
[Serializable]
public class BehaviourDefinition
{
    [SerializeReference, SubclassSelector] public BehaviourNode node;
}

/**
 * Represents a behaviour node that can be used to create a behaviour tree
 * Concrete implementations of this class should be annotated with the [Serializable] attribute and will show as an option in the SubclassSelector
 */
[Serializable]
public abstract class BehaviourNode
{
    [Header("Basic Info")]
    public string nodeName;
    
    public abstract Node GetNode();
}

#region BehaviourNodes
/**
 * Doorway will move to the target location
 */
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

/**
 * Doorway will play an animation, and wait for the animation to finish
 */
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
        return !isAnimatedOrTransition;
    }
}

/**
 * Will Debug.Log the node name
 */
[Serializable]
public class DebugNode : BehaviourNode
{
    public override Node GetNode()
    {
        return new DebugLeaf(nodeName);
    }
}

/**
 * Will wait for a certain amount of time in seconds
 */
[Serializable]
public class WaitNode : BehaviourNode
{
    [Header("Requirements")]
    [SerializeField] public float waitTimeInSeconds;
    
    public override Node GetNode()
    {
        return new WaitLeaf(waitTimeInSeconds);
    }
}

/**
 * Will wait for the player to move to a certain location
 */
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

/**
 * Doorway will play a Dialog, and wait for the dialog to finish
 */
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

/**
 * Doorway will rotate to look at the target
 */
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