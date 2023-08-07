using System;
using MonoWaves.QoL;
using UnityEngine;

public class Deformer : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField, Tooltip("Оставьте пустое поле, если не надо менять коллайдер")] private MeshCollider _meshCollider;

    [Header("Properties")]
    [SerializeField] private float _impactMultiplier = 0.25f;
    [SerializeField] private float _deformRadius = 0.65f;
    [SerializeField] private Vector3 _impactDivider;
    [SerializeField] private Vector3 _impactClamper;

    [Header("Debug")]
    [SerializeField] private bool _doLog;

    private Mesh _target;
    private Vector3[] _modifiedVerts;

    public void Deform(Vector3 point, Vector3 impact)
    {
        Vector3 localPoint = transform.InverseTransformPoint(point);
        Vector3 impactRecalculated = _impactMultiplier * impact.Divide(_impactDivider).Clamp(-_impactClamper, _impactClamper);

        for (int vertex = 0; vertex < _modifiedVerts.Length; vertex++)
        {
            float distanceToPoint = Vector3.Distance(_modifiedVerts[vertex], localPoint);

            if (distanceToPoint < _deformRadius)
            {
                float distanceFalloff = 1 - Mathf.Clamp(distanceToPoint, 0, _deformRadius) / _deformRadius;

                Vector3 deformForce = distanceFalloff * impactRecalculated;
                _modifiedVerts[vertex] += deformForce;
            }
        }

        RecalculateMesh();
    }

    private void Awake()
    {
        if (_meshFilter == null) throw new ArgumentException("Target mesh filter is NULL");

        _target = _meshFilter.mesh;
        _modifiedVerts = _target.vertices;
    }

    private void RecalculateMesh()
    {
        _target.SetVertices(_modifiedVerts);
        _target.RecalculateNormals();

        if (_meshCollider != null)
        {
            _meshCollider.sharedMesh = _target;
        }
    }
}
