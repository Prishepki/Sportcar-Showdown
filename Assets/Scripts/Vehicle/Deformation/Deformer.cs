using System;
using MonoWaves.QoL;
using UnityEngine;

public class Deformer : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField, Tooltip("Оставьте пустое поле, если не надо менять коллайдер")] private MeshCollider _meshCollider;

    [Header("Properties")]
    [SerializeField] private float _deformForceMultiplier = 0.25f;
    [SerializeField] private float _deformRadius = 0.5f;
    [SerializeField] private float _impactMultiplier = 0.05f;
    [SerializeField] private Vector3 _impactDivider;
    [SerializeField] private Vector3 _impactClamper;

    [Header("Debug")]
    [SerializeField] private bool _doLog;

    private Mesh _target;
    private Vector3[] _modifiedVerts;

    public void Deform(Vector3 point, Vector3 impact)
    {
        var clampedImpact = (impact * _impactMultiplier).Divide(_impactDivider).Clamp(-_impactClamper, _impactClamper);
        if (_doLog) Debug.Log($"Deform attempt at {point} | {impact} | {clampedImpact}");

        if (clampedImpact.Equals(0, 0, 0)) return;

        for (int idx = 0; idx < _modifiedVerts.Length; idx++)
        {
            var distance = Vector3.Distance(point, transform.position + _modifiedVerts[idx]);
            var force = _deformForceMultiplier * (1 - Mathf.Clamp(distance, 0, _deformRadius) / _deformRadius) * clampedImpact;

            if (force.magnitude > 0)
            {
                _modifiedVerts[idx] += force;
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
