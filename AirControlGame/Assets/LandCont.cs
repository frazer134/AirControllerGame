using Assets.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class LandCont : MonoBehaviour
{

    public bool longLand = false;
    public bool landing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (longLand == true)
        {
            if (collision.CompareTag("ApproachLong"))
            {
                LandPlane(collision);
            }
            else
            {
                Debug.Log("Long Land Needed");
            }
        }
        else
        {
            if(collision.CompareTag("ApproachLong")||collision.CompareTag("Approach"))
            {
                LandPlane(collision);
            }
        }

        if(landing == true)
        {
            if(collision.CompareTag("RunwayEnd"))
            {
                gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            }
        }
    }

    public void LandPlane(Collider2D collision)
    {
        var runwayShell = collision.gameObject.transform.parent;
        var runwayEnd = runwayShell.transform.Find("RunwayEnd");

        var navPoints = new List<Vector3>();
        navPoints.Add(gameObject.transform.position);
        navPoints.Add(runwayEnd.transform.position);
        var lSpline = SplineMaker.SplineGenerator(navPoints, null);

        gameObject.GetComponent<SplineAnimate>().Container = lSpline;
        gameObject.GetComponent<SplineAnimate>().Restart(false);
        gameObject.GetComponent<SplineAnimate>().Play();

        landing = true;
    }
}
