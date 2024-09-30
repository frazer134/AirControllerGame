using Assets.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class LandCont : MonoBehaviour
{

    public bool longLand = false;
    public bool landing = false;

    public bool inAir = true;

    public bool firstCol = false;

    public float speed = 2f;
    public float scale = 2f;

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
                }
                else if(landing == false)
                {
                    LandPlane(collision);
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
                }
                else if(landing == false)
                {
                    LandPlane(collision);
                }
            }
        }
    }

    public void LandPlane(Collider2D collision)
    {
        var runwayShell = collision.gameObject.transform.parent;
        var runwayStart = runwayShell.transform.Find("TakeoffStart");
        var runwayEnd = runwayShell.transform.Find("RunwayEnd");

        var navPoints = new List<Vector3>();
        navPoints.Add(gameObject.transform.position);
        navPoints.Add(runwayStart.transform.position);
        navPoints.Add(runwayEnd.transform.position);

        var rot = new List<Quaternion>();
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
}
