using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Quaternion = UnityEngine.Quaternion;

namespace Assets.Scenes
{
    public static class SplineMaker
    {

        public static SplineContainer SplineGenerator(List<UnityEngine.Vector3> pathPoints, List<Quaternion> pathRot,  Mesh defaultMesh)
        {
            GameObject nSpline = new GameObject("nSpline");

            UnityEngine.Quaternion defaulRot = new UnityEngine.Quaternion(0,0,0,0);
            var container = nSpline.AddComponent<SplineContainer>();
            //var spline = container.AddSpline();
            var knots = new BezierKnot[pathPoints.Count];
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(0,270, 0);
            //Debug.Log(knots);

            for (int i = 0; i < pathPoints.Count; i++)
            {
                knots[i] = new BezierKnot(pathPoints[i], new UnityEngine.Vector3(0, 0, 0), new UnityEngine.Vector3(0, 0, 0), pathRot[i]);
                //Debug.Log(knots[i]);
            }
            UnityEngine.Splines.Spline spline = new UnityEngine.Splines.Spline(knots, false);
            container.Spline= spline;
            spline.SetTangentMode(TangentMode.AutoSmooth);
            /**
            defaulRot = spline.ToArray()[0].Rotation;

            
            for(int k = 0; k < spline.Knots.Count<BezierKnot>(); k++)
            {
                var firstKnot = spline.ToArray()[k];

                if(firstKnot.Rotation != defaulRot)
                {
                    firstKnot.Rotation = defaulRot;
                    spline.SetKnot(k, firstKnot);
                }
            }
            //Debug.Log(spline);
            **/

            if (defaultMesh != null)
            {
                nSpline.AddComponent<SplineExtrude>();
                nSpline.GetComponent<MeshFilter>().mesh = defaultMesh;
                nSpline.GetComponent<SplineExtrude>().Container = container;
                nSpline.GetComponent<SplineExtrude>().Radius = 0.1f;
                //nSpline.GetComponent<SplineExtrude>().RebuildOnSplineChange = true;
                nSpline.GetComponent<SplineExtrude>().enabled = false;
                nSpline.GetComponent<MeshRenderer>().enabled = false;
            }

            return container;
        }

    }
}
