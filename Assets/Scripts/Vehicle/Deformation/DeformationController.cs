using UnityEngine;

[RequireComponent(typeof(CollisionDetector), typeof(Deformer))]
public class DeformationController : MonoBehaviour
{
    [SerializeField] private CollisionDetector _collisionDetector;
    [SerializeField] private Deformer _deformer;

    private void OnValidate()
    {
        TryGetComponent(out _collisionDetector);
        TryGetComponent(out _deformer);
    }

    private void Awake()
    {
        _collisionDetector.OnCollisionDetected.AddListener(_deformer.Deform);
    }
}
