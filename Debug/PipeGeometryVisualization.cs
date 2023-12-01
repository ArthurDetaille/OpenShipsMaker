using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeGeometryVisualization : MonoBehaviour {
    [Header("Geometry propreties")]
    [Range(3, 16)]      public int resolution = 15;
    [Range(0f, 1f)]     public float radius = 1f;
    [Range(0f, 1f)]     public float padding = 1f;
    public float width = 0.1f;
    [Range(3, 16)]      public int res_pipes = 8;
    
    [Header("Visu")]
    [Range(0.025f, 0.1f)]     public float sphere_width = 0.05f;

    [Header("Handles")]
    public Transform end;

    private void OnDrawGizmos() {
        return;
        float distance = (this.transform.position - end.position).magnitude;
        
        PointAndDirection[] curvePoints = PipeGeometryGenerator.GenerateCurveRelative(resolution, radius, padding, distance);
        Vector3[] verticies = PipeGeometryGenerator.GenerateVerticesRelative(curvePoints, resolution, res_pipes, radius, padding, distance, width);

        Gizmos.color = Color.white;
        foreach (Vector3 vertex in verticies) { Gizmos.DrawWireSphere(vertex, sphere_width); }
    }

    private void Start() {
        GenerateMesh();
    }

    private void Update() {
        GenerateMesh();
        end.transform.position = new Vector3(1f * Mathf.Cos(Time.time * 2f), 0f, 3f * Mathf.Sin(Time.time * 1f)) + Vector3.forward * 5f;
    }

    private void GenerateMesh() {
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter == null) return;
        
        float distance = (this.transform.position - end.position).magnitude;
        filter.mesh = PipeGeometryGenerator.GenerateMeshRelative(resolution, res_pipes, radius, padding, distance, width);

        this.transform.rotation = Quaternion.LookRotation(end.position - this.transform.position);
    }
}