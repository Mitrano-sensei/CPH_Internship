using DG.Tweening;
using UnityEngine;

public class DoorwayTeleportation : MonoBehaviour, ILocomotionSystem
{
    [SerializeField] private Transform doorWay;
    [SerializeField] private GameObject doorWayVisuals;
    [SerializeField] private float teleportationTimeInSeconds = 2f;
    [SerializeField] private float rotationTimeInSeconds = 2f;
    
    private bool _isMoving;

    public bool IsMoving => _isMoving;
    
    public void MoveTo(Vector3 position)
    {
        if (_isMoving)
        {
            Debug.LogError("Doorway is already moving");
            return;
        }
        
        _isMoving = true;
        
        var doorWayVisualsTransform = doorWayVisuals.transform;
        var rotation = doorWayVisualsTransform.localRotation;
        
        var teleportationSequence = DOTween.Sequence();
        teleportationSequence.Append(doorWayVisuals.transform.DOScale(Vector3.zero, teleportationTimeInSeconds/2f));
        teleportationSequence.Join(doorWayVisuals.transform.DOLocalRotate(Vector3.up*90f, teleportationTimeInSeconds/2f));
        teleportationSequence.AppendCallback(() =>
        {
            doorWay.position = position;
            doorWayVisuals.transform.localScale = Vector3.zero;
        });
        teleportationSequence.Append(doorWayVisuals.transform.DOScale(Vector3.one, teleportationTimeInSeconds/2f));
        teleportationSequence.Join(doorWayVisuals.transform.DORotateQuaternion(rotation, teleportationTimeInSeconds/2f));
        teleportationSequence.AppendCallback(() => _isMoving = false);
        
        teleportationSequence.Play();
    }

    public void MoveTo(Transform target) => MoveTo(target.position);
    
    public void RotateTo(Vector3 direction) {
        if (_isMoving)
        {
            Debug.LogError("Doorway is already moving");
            return;
        }
        
        _isMoving = true;
        
        var teleportationSequence = DOTween.Sequence();
        teleportationSequence.Append(doorWay.transform.DOLookAt(transform.position , rotationTimeInSeconds));
        teleportationSequence.AppendCallback(() => _isMoving = false);
        
        teleportationSequence.Play();
    }
    public void RotateTo(Transform target) => RotateTo(target.position - transform.position);
}