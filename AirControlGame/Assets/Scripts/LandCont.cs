using Assets.Scenes;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Splines;

public class LandCont : MonoBehaviour
{

    public bool longLand = false;
    public bool landing = false;

    public bool inAir = true;

    public bool firstCol = false;

    public float speed = 2f;
    public float scale = 2f;

    public AudioClip landClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (landing == true)
        {
            if (gameObject.GetComponent<MoveAlongSpline>().GetDistance() == 0 && gameObject.GetComponent<MoveAlongSpline>().moving == false)
            {
                gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            }

            var norTime = gameObject.GetComponent<MoveAlongSpline>().GetDistance();
            var altSpeed = speed - (speed * norTime);
            var altScale = scale - (scale * norTime);
            if (altSpeed < 1.5f)
            {
                altSpeed = 1.5f;
            }
            if(altScale < 1f)
            {
                altScale = 1f;
            }
            gameObject.GetComponent<MoveAlongSpline>().speed = altSpeed;
            gameObject.transform.localScale = new Vector3(altScale, altScale, altScale);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (longLand == true)
        {
            if (collision.CompareTag("ApproachLong"))
            {
                if (landing == false && firstCol == false)
                {
                    firstCol = true;
                    var tempG = collision.gameObject.transform.parent;
                    Vector3 centPos = FindCenterPoint(tempG.transform.parent.Find("Center").gameObject);
                    LandPlane(collision,centPos);
                }
            }
            else
            {
                if (landing == false)
                {
                    gameObject.GetComponent<SplineGen>().WrongRunway();
                }
            }
        }
        else
        {
            if(collision.CompareTag("ApproachLong")||collision.CompareTag("Approach"))
            {
                if (landing == false && firstCol == false)
                {
                    firstCol = true;
                    var tempG = collision.gameObject.transform.parent;
                    Vector3 centPos = FindCenterPoint(tempG.transform.parent.Find("Center").gameObject);
                    LandPlane(collision, centPos);
                }
            }
        }

        if(collision.CompareTag("CenterLand"))
        {
            if(landing == false && firstCol == true)
            {
                //LandPlane(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CenterLand") && landing == true)
        {
            Debug.Log("Hitbox Hit");
            gameObject.GetComponent<SplineGen>().inAir = false;
        }
    }

    public void LandPlane(Collider2D collision, Vector3 centerP)
    {
        gameObject.GetComponent<AudioSource>().clip = landClip;
        gameObject.GetComponent<AudioSource>().Play();

        var runwayShell = collision.gameObject.transform.parent.gameObject.transform.parent;
        var runwayStart = runwayShell.transform.Find("TakeoffStart");
        var runwayEnd = runwayShell.transform.Find("RunwayEnd");

        var navPoints = new List<Vector3>();
        navPoints.Add(gameObject.transform.position);
        navPoints.Add(centerP);
        navPoints.Add(runwayStart.transform.position);
        navPoints.Add(runwayEnd.transform.position);

        var rot = new List<Quaternion>();
        rot.Add(new Quaternion(0, 0, 0, 0));
        rot.Add(new Quaternion(0, 0, 0, 0));
        rot.Add(new Quaternion(0, 0, 0, 0));
        rot.Add(new Quaternion(0, 0, 0, 0));
        var lSpline = SplineMaker.SplineGenerator(navPoints, rot, null);

        gameObject.GetComponent<MoveAlongSpline>().SplineUpadte(lSpline);
        gameObject.GetComponent<MoveAlongSpline>().moving = true;

        landing = true;

        var norTime = gameObject.GetComponent<MoveAlongSpline>().GetDistance();
        var altSpeed = speed - (speed * norTime);
        if(altSpeed < 0.2f)
        {
            speed = 0.2f;
        }
        gameObject.GetComponent<MoveAlongSpline>().speed = altSpeed;
    }

    private Vector3 FindCenterPoint(GameObject center)
    {
        /**
        float dist = 10000;
        Vector3 knot = new Vector3(0,0,0);
        var splineC = center.GetComponent<SplineContainer>().Spline;
        foreach(BezierKnot k in splineC.Knots)
        {
            if(Vector3.Distance(gameObject.transform.position, k.Position) < dist)
            {
                dist = Vector3.Distance(gameObject.transform.position, k.Position);
                knot = k.Position;
            }
        }

        //knot = center.transform.TransformVector(knot);
        return knot;
        **/
        var splineC = center.GetComponent<SplineContainer>().Spline;

        SplineUtility.GetNearestPoint(splineC, gameObject.transform.position, out float3 nearest, out float t);

        Vector3 knot = new Vector3(nearest.x, nearest.y, nearest.z);
        //knot = center.transform.worldToLocalMatrix * knot;

        return knot;
    }
}
