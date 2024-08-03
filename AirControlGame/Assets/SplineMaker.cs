using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

namespace Assets.Scenes
{
    public static class SplineMaker
    {
        public static SplineContainer SplineGenerator(List<UnityEngine.Vector3> pathPoints, Mesh defaultMesh)
        {
            GameObject nSpline = new GameObject("nSpline");

            var container = nSpline.AddComponent<SplineContainer>();
            var spline = container.AddSpline();
            var knots = new BezierKnot[pathPoints.Count];
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(0, -90, 0);
            //Debug.Log(knots);

            for (int i = 0; i < pathPoints.Count; i++)
            {
                knots[i] = new BezierKnot(pathPoints[i], new UnityEngine.Vector3(0,0,0), new UnityEngine.Vector3(0,0,0), rot);
            }

            spline.Knots = knots;
            spline.SetTangentMode(TangentMode.AutoSmooth);
            //Debug.Log(spline);

            nSpline.AddComponent<SplineExtrude>();
            nSpline.GetComponent<MeshFilter>().mesh = defaultMesh;
            nSpline.GetComponent<SplineExtrude>().Container = container;
            nSpline.GetComponent<SplineExtrude>().Radius = 0.1f;
            //nSpline.GetComponent<SplineExtrude>().RebuildOnSplineChange = true;
            nSpline.GetComponent<SplineExtrude>().enabled = false;
            nSpline.GetComponent<MeshRenderer>().enabled = false;

            return container;
        }

    }
}
