using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public readonly struct PipeProps {
//     public PipeProps(Transform from, Transform to) {
//         From = from; To = to;
//         Resolution = 8;
//         Radius = 0.2f;
//         BaseRadius = 0.5f;
//         BaseWidth = 0.1f;
//         Padding = 0.5f;
//     }

//     public int Resolution { get; init; }
//     public float Radius { get; init; }
//     public float BaseRadius { get; init; }
//     public float BaseWidth { get; init; }
//     public float Padding { get; init; }

//     public Transform From { get; init; }
//     public Transform To { get; init; }
// }

public enum BaseGenerationType { FROM, TO }

public class PipeCreator : MonoBehaviour
{

    // public static List<Vector3> create_section(BezierPoint point, int res, float radius) {
    //     List<Vector3> points = new List<Vector3>();
        
    //     if (res < 3) res = 3;
        
    //     for (int i = 0; i < res; i++) {
    //         float theta = 2 * Mathf.PI * (float)i/(float)res;
    //         points.Add(point.position + new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta)));
    //     }

    //     return points;
    // }

    // public static List<Vector3> create_verticies(PipeProps proprieties) {
    //     List<Vector3> pipe_verticies = new List<Vector3>();

    //     pipe_verticies.AddRange(create_base(
    //         BaseGenerationType.FROM,
    //         proprieties
    //     ));

    //     return pipe_verticies;
    // }

    // public static Vector3[] create_base(BaseGenerationType type, PipeProps proprieties) {
    //     List<Vector3> points = new List<Vector3>();

    //     BezierPoint working_point = type == BaseGenerationType.FROM ? proprieties.From : proprieties.To;

    //     points.AddRange(create_section(
    //         working_point,
    //         proprieties.Resolution,
    //         proprieties.BaseRadius
    //     ));

    //     working_point.position += point.direction * proprieties.BaseWidth;
    //     points.AddRange(create_section(
    //         working_point,
    //         proprieties.Resolution,
    //         proprieties.BaseRadius
    //     ));

    //     points.AddRange(create_section(
    //         working_point,
    //         proprieties.Resolution,
    //         proprieties.Radius
    //     ));
        
    //     working_point.position += point.direction * (proprieties.BaseWidth + proprieties.Padding);
    //     points.AddRange(create_section(
    //         working_point,
    //         proprieties.Resolution,
    //         proprieties.Radius
    //     ));
        
    //     return points;
    // }
}
