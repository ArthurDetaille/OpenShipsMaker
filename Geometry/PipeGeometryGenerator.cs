using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PointAndDirection {
    public Vector3 position;
    public Vector3 direction;
    public float theta;
    public bool reflected;

    public PointAndDirection(Vector3 pos, Vector3 dir, float theta, bool reflected = false) {
        this.position = pos;
        this.direction = dir;
        this.theta = theta;
        this.reflected = reflected;
    }
}

public class PipeGeometryGenerator : MonoBehaviour {
    // TODO : commenting

    public static PointAndDirection[] GenerateBendRelative(int res, float radius, float padding, float distance = 0f, bool reflected = false) {
        // Cette fonction sert à générer les points (c'est-à-dire une position et une direction) dans un coude d'un tuyau [traduit en 'pipe']
        // Cela est fait à travers un arc-de-cercle (d'où l'apparition des mots 'radius')
        // cela est fait à la hauteur 'padding' et la distance depuis l'origine 'distance'
        // aussi, nous sommes en mesure de faire sa réflection (pour la deuxième coudée du tuyau)

        PointAndDirection[] points = new PointAndDirection[res];
        for (int i = 0; i < res; i++) {
            int offset = reflected ? 0 : 1;
            float t = (float)(i + offset) / (float)res;
            float theta = Mathf.PI / 2f * t;

            float reflection = (reflected ? -1f : 1f);

            float x = radius * (1f - Mathf.Cos(theta)) * reflection;
            float y = radius * Mathf.Sin(theta);

            Vector3 position = new Vector3(0f, y + padding, x + distance);
            Vector3 direction = new Vector3(0f, Mathf.Cos(theta) * reflection, Mathf.Sin(theta));

            PointAndDirection point = new PointAndDirection(position, direction, -theta * reflection, reflected);

            points[i] = point;
        }
        if (reflected) return invert(points);

        return points;
    }

    private static PointAndDirection[] GenerateStraight(float length, float distance = 0f, bool reflected = false) {
        PointAndDirection[] points = new PointAndDirection[2];

        float reflection = reflected ? -1f : 1f;

        PointAndDirection p1 = new PointAndDirection(distance * Vector3.forward, Vector3.up * reflection, 0f);
        PointAndDirection p2 = new PointAndDirection(distance * Vector3.forward + length * Vector3.up, Vector3.up * reflection, 0f);
        
        points[reflected ? 1 : 0] = p1;
        points[reflected ? 0 : 1] = p2;

        return points;
    }

    public static PointAndDirection[] GenerateCurveRelative(int res, float radius, float padding, float length) {
        // this may seem random as a number of points
        // but 2 bend (2 * res points) and 2 padding (2 * 2 points)
        // this is the best explanation I can deliver by text

        // C'est sans doute la pire chose que j'ai écrit de toute ma carrière
        // tant pis, j'en ai juste trop marre de bosser sur ce problème qui ne fini jamais
        // Ceux qui me juge : sucez mes boules svp
        List<PointAndDirection> points = new List<PointAndDirection>();
        points.AddRange(GenerateStraight(padding));
        points.AddRange(GenerateBendRelative(res, radius, padding));

        Vector3 pos1 = Vector3.forward * (length - radius) + Vector3.up * (padding + radius); 
        points.Add(new PointAndDirection(pos1, Vector3.forward, Mathf.PI / 2f));

        points.AddRange(GenerateBendRelative(res, radius, padding, length, true));

        Vector3 pos2 = Vector3.forward * length;
        points.Add(new PointAndDirection(pos2, - Vector3.up, Mathf.PI));

        return points.ToArray();
    }

    private static PointAndDirection[] invert(PointAndDirection[] arr) {
        PointAndDirection[] inverted = new PointAndDirection[arr.Length];
        for (int i =0; i < arr.Length; i++) {
            inverted[i] = arr[arr.Length - i - 1];
        }

        return inverted;
    }

    public static Vector3[] GenerateVerticesRelative(PointAndDirection[] curvePoints, int res, int res_pipes, float radius, float padding, float distance, float width) {
        Vector3[] verticies = new Vector3[curvePoints.Length * res_pipes];

        for (int i = 0; i < curvePoints.Length; i++) {
            float reflection = 1f;
            if (i < curvePoints.Length - 1) reflection = curvePoints[i + 1].reflected ? -1f : 1f;

            for (int j = 0; j < res_pipes; j++) {
                Vector3 position = curvePoints[i].position;

                float t = (float)j / (float) res_pipes;
                float alpha = Mathf.PI * 2f * t * reflection;

                Vector3 pipe_offset = new Vector3(
                    Mathf.Sin(alpha),
                    Mathf.Cos(alpha) * Mathf.Sin(curvePoints[i].theta),
                    Mathf.Cos(alpha) * Mathf.Cos(curvePoints[i].theta)
                );

                // TODO : some triangles are inverted. This is probably caused by the fact that vertices are
                // in the wrong order.

                verticies[i * res_pipes + j] = position + width * pipe_offset;
            }
        }

        return verticies;
    }

    public static int[] GenerateTrianglesRelative(PointAndDirection[] curvePoints, int res_pipe) {
        int curvePointsCount = curvePoints.Length;
        int[] triangles = new int[curvePointsCount * res_pipe * 6];
        for (int j = 0; j < curvePointsCount - 1; j++) {
            for (int i = 0; i < res_pipe; i++) {
                int index = j * res_pipe * 6 + i * 3;
                triangles[index + 0]    = j * res_pipe      + i%res_pipe;
                triangles[index + 1]    = j * res_pipe      + (i+1)%res_pipe;
                triangles[index + 2]    = (j+1) * res_pipe  + i%res_pipe;

                index += res_pipe * 3;
                triangles[index + 0]    = (j+1) * res_pipe  + (i+1)%res_pipe;
                triangles[index + 1]    = (j+1) * res_pipe      + i%res_pipe;
                triangles[index + 2 ]   = j * res_pipe      + (i+1)%res_pipe;
            }
        }
        

        return triangles;
    }

    public static Mesh GenerateMeshRelative(int res, int res_pipes, float radius, float padding, float distance, float width) {
        Mesh mesh = new Mesh();

        PointAndDirection[] curvePoints = GenerateCurveRelative(res, radius, padding, distance);
        Vector3[] vertices = GenerateVerticesRelative(curvePoints, res, res_pipes, radius, padding, distance, width);
        int[] triangles = GenerateTrianglesRelative(curvePoints, res_pipes);

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}