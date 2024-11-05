using Assets.Scenes;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class LandCont : MonoBehaviour
{

    public bool longLand = false;
    public bool landing = false;

    public bool inAir = false;

    public bool outside = true;

    public float speed = 2f;
    public float scale = 2f;

    public AudioClip landClip;
    public Material pathMat;
    public Mesh mesh;

    public bool onEdge = false;
    public bool alarmActive = false;

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

        if(onEdge == true)
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * 10f);
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.right, 10f, LayerMask.GetMask("FailLayer"));
            if (hit1.collider != null)
            {
                if (hit1.collider.gameObject.CompareTag("FailCollider"))
                {
                    if (alarmActive == false)
                    {
                        gameObject.GetComponent<SplineGen>().AlarmOn();
                        alarmActive = true;
                    }
                }
                else
                {
                    gameObject.GetComponent<SplineGen>().AlarmOff();
                    alarmActive = false;
                }
            }
            else
            {
                gameObject.GetComponent<SplineGen>().AlarmOff();
                alarmActive = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (longLand == true)
        {
            if (collision.CompareTag("ApproachLong"))
            {
                if (landing == false)
                {
                    if (outside)
                    {
                        var tempG = collision.gameObject.transform.parent;
                        Vector3 centPos = FindCenterPoint(tempG.transform.parent.Find("Center").gameObject);
                        LandPlane(collision, centPos);
                        outside = false;
                    }
                    else
                    {
                        outside = true;
                    }
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
                if (landing == false)
                {
                    if (outside)
                    {
                        var tempG = collision.gameObject.transform.parent;
                        Vector3 centPos = FindCenterPoint(tempG.transform.parent.Find("Center").gameObject);
                        LandPlane(collision, centPos);
                        outside = false;
                    }
                    else
                    {
                        outside= true;
                    }
                }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GoalCollider"))
        {
            if (landing == false)
            {
                onEdge = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GoalCollider"))
        {
            if (landing == false)
            {
                onEdge = false;
            }
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
        var lSpline = SplineMaker.SplineGenerator(navPoints, rot, mesh);

        gameObject.GetComponent<MoveAlongSpline>().SplineUpadte(lSpline);
        gameObject.GetComponent<MoveAlongSpline>().moving = true;

        if (gameObject.GetComponent<SplineGen>().nSpline != null)
        {
            var oldSpline = gameObject.GetComponent<SplineGen>().nSpline;
            gameObject.GetComponent<SplineGen>().nSpline = lSpline;
            Destroy(oldSpline.gameObject);
        }

        landing = true;

        var norTime = gameObject.GetComponent<MoveAlongSpline>().GetDistance();
        var altSpeed = speed - (speed * norTime);
        if(altSpeed < 0.2f)
        {
            speed = 0.2f;
        }
        gameObject.GetComponent<MoveAlongSpline>().speed = altSpeed;

        if(lSpline.gameObject.GetComponent<SplineExtrude>().enabled == false)
        {
            lSpline.gameObject.GetComponent<MeshRenderer>().material = pathMat;
            lSpline.gameObject.GetComponent<SplineExtrude>().enabled = true;
            lSpline.gameObject.GetComponent<MeshRenderer>().enabled = true;
            lSpline.gameObject.GetComponent<SplineExtrude>().Rebuild();

        }
    }

    private Vector3 FindCenterPoint(GameObject center)
    {
        var splineC = center.GetComponent<SplineContainer>().Spline;

        SplineUtility.GetNearestPoint(splineC, gameObject.transform.position, out float3 nearest, out float t);

        Vector3 knot = new Vector3(nearest.x, nearest.y, 0f);
        //knot = center.transform.worldToLocalMatrix * knot;

        knot = center.transform.localToWorldMatrix.MultiplyPoint(knot);

        return knot;
    }

    public void ResetInAir()
    {
        gameObject.transform.localScale = new Vector3(2, 2, 2);
        speed = 2f;
        inAir = true;
        landing = false;
    }
}
