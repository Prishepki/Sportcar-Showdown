using System;
using UnityEngine;

public class Deformer : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField, Tooltip("Оставьте пустое поле, если не надо менять коллайдер")] private MeshCollider _meshCollider;

    private Mesh _target;
    private Vector3[] _modifiedVerts;

    public void Deform(Vector3 point, Vector3 normal, float impact)
    {
        float clampedImpact = Mathf.Clamp(impact / 10, 0, 6);
        Debug.Log($"Deform attempt {point} {impact} {clampedImpact}");

        for (int idx = 0; idx < _modifiedVerts.Length; idx++)
        {
            float distance = Vector3.Distance(point, transform.position + _modifiedVerts[idx]);

            var force = clampedImpact * (1 - Mathf.Clamp(distance, 0, 0.5f) * 2) / 2;
            //Debug.Log($"{clampedImpact} * (1 - Mathf.Clamp({distance}, 0, 0.5f) * 2) / 2");

            Vector3 deformVector = normal * force;

            _modifiedVerts[idx] += deformVector;
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

        _meshCollider.sharedMesh = _target;
    }
}
