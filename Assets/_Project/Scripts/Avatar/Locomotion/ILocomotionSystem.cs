using UnityEngine;

public interface ILocomotionSystem
{
    bool IsMoving { get; }
    
    void MoveTo(Vector3 position);
    void MoveTo(Transform target);
    void RotateTo(Quaternion rotation);
}