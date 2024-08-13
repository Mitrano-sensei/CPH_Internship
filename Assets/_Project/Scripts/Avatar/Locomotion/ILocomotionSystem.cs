using UnityEngine;

/**
 * Simple interface to easily switch between different locomotion systems, as teleportation or Navmesg
 */
public interface ILocomotionSystem
{
    bool IsMoving { get; }
    
    // Move to the position
    void MoveTo(Vector3 position);

    void MoveTo(Transform target);
    
    // Rotate to look at the target
    void RotateTo(Transform target);
    
    void RotateTo(Vector3 targetPosition);
}