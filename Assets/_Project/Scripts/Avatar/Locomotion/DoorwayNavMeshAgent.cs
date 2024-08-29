using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class DoorwayNavMeshAgent : MonoBehaviour, ILocomotionSystem
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform doorWay;

    [SerializeField] private Animator animator;
    
    [Header("Misc")]
    [SerializeField] private float rotationTimeInSeconds = 2f;

    private bool _isMoving;
    public bool IsMoving => (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.1f) || _isMoving;
    public float RotationTime => rotationTimeInSeconds;

    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    

    public void Start()
    {
        navMeshAgent.updateRotation = true;
    }

    public void Update()
    {
        animator.SetFloat(MovementSpeed, navMeshAgent.velocity.magnitude / navMeshAgent.speed);
    }
    
    public void MoveTo(Transform target) => navMeshAgent.SetDestination(target.position);

    public void MoveTo(Vector3 position) => navMeshAgent.SetDestination(position);

    public void RotateTo(Transform target) => RotateTo(target.position);

    public void RotateTo(Vector3 target)
    {
        if (_isMoving)
        {
            Debug.LogError("Doorway is already moving");
            return;
        }
        
        _isMoving = true;
        
        var teleportationSequence = DOTween.Sequence();
        
        teleportationSequence.Append(doorWay.transform.DOLookAt(target, rotationTimeInSeconds));
        teleportationSequence.AppendCallback(() => _isMoving = false);
        
        teleportationSequence.Play();
    }
}
