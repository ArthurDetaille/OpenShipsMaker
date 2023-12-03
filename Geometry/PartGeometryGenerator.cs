using UnityEngine;
using System;

public abstract class PartGeometryGenerator : MonoBehaviour {
    [SerializeField] protected ControlPoint[] controlPoints;

    // FUNCTIONS
    protected abstract void GenerateGeometry();
    protected abstract void HandleControlPoint(int index);

    // GETTERS AND SETTERS
    public int GetControlPointsCount()                  { return controlPoints.Length; }
    public ControlPoint GetControlPointAtIndex(int i)   { return controlPoints[i]; }
    public ControlPoint[] GetControlPoints()            { return controlPoints; }
}