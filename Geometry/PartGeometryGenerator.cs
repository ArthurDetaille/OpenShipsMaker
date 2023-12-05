using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class PartGeometryGenerator : MonoBehaviour {
    protected ControlPoint[] controlPoints;
    protected Dictionary<string, ControlPoint> identification_table;

    // ABSTRACT FUNCTIONS
    protected abstract void GenerateGeometry();

    // ENGINE FUNCTIONS
    private void Awake() {
        InitializeControlPoints();
        BuildIdentificationDictionary();
    }

    // FUNCTIONS
    protected void BuildIdentificationDictionary() {
        Dictionary<string, ControlPoint> dict = new Dictionary<string, ControlPoint>();
        foreach(ControlPoint point in controlPoints) { dict.Add(point.identification_string, point); }
        identification_table = dict;
    }

    protected void InitializeControlPoints() { controlPoints = this.GetComponentsInChildren<ControlPoint>(); }

    // GETTERS AND SETTERS
    public int GetControlPointsCount()                  { return controlPoints.Length; }
    public ControlPoint GetControlPointById(string id)  { return identification_table[id]; }
    public ControlPoint[] GetControlPoints()            { return controlPoints; }
}