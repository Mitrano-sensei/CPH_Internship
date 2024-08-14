using BehaviourTree;

/**
 * Will Start a new Dialog, and will play each lines until it is finished, then return Success
 * 
 * TODO --> Should replace Process with a IStrategy implementation ? 
 */
public class DialogCompleteLeaf : Leaf
{
    private readonly DialogSystem _dialogSystem;
    private readonly DialogInfo _dialog;

    private bool _hasStarted;
    
    public DialogCompleteLeaf(DialogSystem dialogSystem, DialogInfo dialogInfo, string name = "DialogLineNode", int priority = 0) : base(new NothingStrategy(), name, priority)
    {
        _dialogSystem = dialogSystem;
        _dialog = dialogInfo;
    }

    public override Status Process() {
        // Starts the dialog
        if (!_hasStarted)
        {
            _hasStarted = true;
            _dialogSystem.StartDialog(_dialog);
            return Status.Running;
        }

        // If the dialog is finished, return success
        if (_dialogSystem.IsDialogFinished)
        {
            Reset();
            return Status.Success;
        }

        
        // If the dialog is not typing anymore, go to next Line
        if (!_dialogSystem.IsTyping) 
            _dialogSystem.NextLine();

        // Always return running, and let next Process handle the Success if needed
        return Status.Running;
    }
    
    public override void Reset()
    {
        base.Reset();
        
        _hasStarted = false;
    }
    
}

/**
 * Will Start a new Dialog and play the first line, and then return Success
 *
 * TODO --> Should replace Process with a IStrategy implementation ? 
 */
public class DialogStartLeaf : Leaf
{
    private readonly DialogSystem _dialogSystem;
    private readonly DialogInfo _dialog;
    
    private bool _hasStarted;
    
    public DialogStartLeaf(DialogSystem dialogSystem, DialogInfo dialogInfo, string name = "Leaf", int priority = 0) : base(new NothingStrategy(), name, priority)
    {
        _dialogSystem = dialogSystem;
        _dialog = dialogInfo;
    }

    public override Status Process()
    {
        if (!_hasStarted) HandleStart();
        
        
        if (_dialogSystem.IsTyping) return Status.Running;

        Reset();
        return Status.Success;
    }
    
    private void HandleStart()
    {
        _hasStarted = true;
        _dialogSystem.StartDialog(_dialog);
    }

    public override void Reset()
    {
        base.Reset();
        _hasStarted = false;
    }
}

/**
 * Will play the next line of the dialog, and return Success when the line is finished
 *
 * TODO --> Should replace Process with a IStrategy implementation ? 
 */
public class DialogNextLineLeaf : Leaf
{
    private readonly DialogSystem _dialogSystem;
    
    private bool _hasStarted;
    
    public DialogNextLineLeaf(DialogSystem dialogSystem, string name = "Leaf", int priority = 0) : base(new NothingStrategy(), name, priority)
    {
        _dialogSystem = dialogSystem;
    }
    
    public override Status Process()
    {
        if (!_hasStarted) HandleStart();

        if (_dialogSystem.IsTyping) return Status.Running;

        Reset();
        return Status.Success;
    }
    
    private void HandleStart()
    {
        _hasStarted = true;
        _dialogSystem.NextLine();
    }
    
    public override void Reset()
    {
        base.Reset();
        _hasStarted = false;
    }
}

/**
 * Will play the next line of the dialog, play all remaining lines of the dialog, and return Success when the dialog is finished
 */
public class DialogFinishLeaf : Leaf
{
    private readonly DialogSystem _dialogSystem;

    public DialogFinishLeaf(DialogSystem dialogSystem, string name = "Leaf", int priority = 0) : base(new NothingStrategy(), name, priority)
    {
        _dialogSystem = dialogSystem;
    }

    public override Status Process()
    {
        if (_dialogSystem.IsDialogFinished)
        {
            Reset();
            return Status.Success;
        }

        if (!_dialogSystem.IsTyping) _dialogSystem.NextLine();
        
        return Status.Running;
    }
}