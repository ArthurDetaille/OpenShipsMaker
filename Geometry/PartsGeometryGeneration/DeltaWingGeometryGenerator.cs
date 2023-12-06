using UnityEngine;
using System.Collections.Generic;

public class DeltaWingGeometryGenerator : PartGeometryGenerator {
    
    public static float WING_LENGTH     = 2.0f;
    public static float WING_WIDTH      = 0.1f;
    public static float TIP_Z           = .5f;
    public static float ATTACK_LENGTH   = .2f;

    protected override Mesh GenerateGeometry() {
        Debug.Log("Generating mesh for Delta wing.");
        
        Mesh mesh = new Mesh();
        ControlPoint cp = GetControlPointById("delta_tip");
        
        Vector3[] vertices  = GenerateVertices(cp);
        int[] triangles     = GenerateTriangles();

        mesh.Clear();

        mesh.vertices   = vertices;
        mesh.triangles  = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    private Vector3[] GenerateVertices(ControlPoint cp) {
        float cpx = cp.transform.localPosition.x;
        List<Vector3> vertices = new List<Vector3>();
        
        Vector3 f = this.transform.forward;
        Vector3 r = this.transform.right;
        Vector3 u = this.transform.up;

        Vector3 z = Vector3.zero;

        vertices.Add(-f * WING_LENGTH                       + z * cpx);                             // 0        2 --- 3
        vertices.Add(-f * WING_LENGTH                       + r * cpx);                             // 1        |  /  |
                                                                                                    //          0 --- 1
        vertices.Add(-f * (WING_LENGTH - ATTACK_LENGTH)     + z * cpx   + u * 0.5f * WING_WIDTH);   // 2        |  /  |
        vertices.Add(-f * (WING_LENGTH - ATTACK_LENGTH)     + r * cpx   + u * 0.5f * WING_WIDTH);   // 3        4 --- 5
                                                                                                    //      WING BACK VIEW
        vertices.Add(-f * (WING_LENGTH - ATTACK_LENGTH)     + z * cpx   - u * 0.5f * WING_WIDTH);   // 4
        vertices.Add(-f * (WING_LENGTH - ATTACK_LENGTH)     + r * cpx   - u * 0.5f * WING_WIDTH);   // 5

        vertices.Add( f * WING_LENGTH                       + z * cpx);                             // 6        3 --- 8
        vertices.Add(-f * (WING_LENGTH - 2 * ATTACK_LENGTH) + r * cpx);                             // 7        |  /  |
                                                                                                    //          7 --- 6
        vertices.Add( f * (WING_LENGTH - ATTACK_LENGTH)     + z * cpx   + u * 0.5f * WING_WIDTH);   // 8        |  /  |
        vertices.Add( f * (WING_LENGTH - ATTACK_LENGTH)     + z * cpx   - u * 0.5f * WING_WIDTH);   // 9        5 --- 9
                                                                                                    //      WING FRONT VIEW

        return vertices.ToArray();
    }

    private int[] GenerateTriangles() {
        List<int> triangles = new List<int>();

        triangles.AddRange(new int[]{0, 2, 3}); // trailing edge top - 1
        triangles.AddRange(new int[]{0, 3, 1}); // trailing edge top - 2

        triangles.AddRange(new int[]{4, 0, 1}); // trailing edge bottom - 1
        triangles.AddRange(new int[]{4, 1, 5}); // trailing edge bottom - 2

        triangles.AddRange(new int[]{5, 1, 3}); // lil triangle back
        triangles.AddRange(new int[]{5, 3, 7}); // lil triangle front

        triangles.AddRange(new int[]{7, 3, 8}); // attack edge top - 1
        triangles.AddRange(new int[]{7, 8, 6}); // attack edge top - 2

        triangles.AddRange(new int[]{2, 8, 3}); // surface top
        triangles.AddRange(new int[]{4, 5, 9}); // surface bottom

        triangles.AddRange(new int[]{5, 6, 9}); // attack edge bottom - 1
        triangles.AddRange(new int[]{5, 7, 6}); // attack edge bottom - 2
    
        return triangles.ToArray();
    }
}