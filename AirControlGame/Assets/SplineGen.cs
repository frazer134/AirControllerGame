using Assets.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class SplineGen : MonoBehaviour
{
    public Quaternion startRot;
    //[SerializeField] private Vector3[] splinePoints;

    public SplineContainer nSpline;

    public bool splineGenerated = false;
    public bool onApproach = false;
    public bool landed = false;

    GameObject oldSpline = null;

    public float forwardSpeed = 1f;
    public bool paused = false;

    public bool LongLand = false;

    public GameObject uiCanvas;

    public bool TakeoffPlane = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = startRot;
        MouseCont.pauseG += PausePlane;
        MouseCont.startG += StartPlane;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == false)
        {
            transform.position = transform.position + ((transform.right * forwardSpeed) * Time.deltaTime);
            /**
            if (gameObject.GetComponent<SplineAnimate>().IsPlaying == false)
            {
                transform.position = transform.position + ((transform.right * forwardSpeed)*Time.deltaTime);
            }
            else
            {
                //gameObject.GetComponent<SplineAnimate>().Play();
            }
            **/
        }
    }

    public void GenPlanePath(List<Vector3> planePath, Mesh defaultMesh)
    {

        nSpline = SplineMaker.SplineGenerator(planePath, defaultMesh);

        gameObject.GetComponent<SplineAnimate>().Container = nSpline;
        gameObject.GetComponent<SplineAnimate>().Restart(false);
        gameObject.GetComponent<SplineAnimate>().Play();

        splineGenerated = true;

        SplineCleanUp(nSpline);
    }

    public void SplineCleanUp(SplineContainer cSpline)
    {
        if (splineGenerated)
        {
            if (oldSpline != null)
            {
                Destroy(oldSpline);
            }

            oldSpline = cSpline.gameObject;

            splineGenerated= false;
        }
    }

    public void PausePlane()
    {
        Debug.Log("Plane Stopped");
        gameObject.GetComponent<SplineAnimate>().Pause();
        paused= true;
    }

    public void StartPlane()
    {
        gameObject.GetComponent<SplineAnimate>().Play();
        paused= false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collison Detection & Sequence for landing plane

        /**
        if (TakeoffPlane == false)
        {
            if (collision.CompareTag("ApproachLong"))
            {
                if (onApproach == false)
                {
                    var runwayS = collision.gameObject.GetComponent<SplineContainer>().Spline;
                    var start = runwayS.ToArray()[1];
                    var end = runwayS.ToArray()[0];
                    var plane = (gameObject.transform.position) - collision.gameObject.transform.position;

                    var offset = (end.Position.x - plane.x) / (end.Position.x - start.Position.x);

                    onApproach = true;

                    gameObject.GetComponent<SplineAnimate>().StartOffset = offset;
                    gameObject.GetComponent<SplineAnimate>().Container = collision.gameObject.GetComponent<SplineContainer>();
                    gameObject.GetComponent<SplineAnimate>().Restart(false);
                    gameObject.GetComponent<SplineAnimate>().Play();
                }
            }

            if (collision.CompareTag("Approach"))
            {
                if (onApproach == false)
                {
                    if (LongLand == false)
                    {
                        var runwayS = collision.gameObject.GetComponent<SplineContainer>().Spline;
                        var start = runwayS.ToArray()[1];
                        var end = runwayS.ToArray()[0];
                        var plane = (gameObject.transform.position) - collision.gameObject.transform.position;

                        var offset = (end.Position.x - plane.x) / (end.Position.x - start.Position.x);

                        onApproach = true;

                        gameObject.GetComponent<SplineAnimate>().StartOffset = offset;
                        gameObject.GetComponent<SplineAnimate>().Container = collision.gameObject.GetComponent<SplineContainer>();
                        gameObject.GetComponent<SplineAnimate>().Restart(false);
                        gameObject.GetComponent<SplineAnimate>().Play();
                    }
                    else
                    {
                        print("Long Land Needed");
                    }
                }
                else if (onApproach == true)
                {
                    gameObject.GetComponent<SplineAnimate>().StartOffset = 0;
                    gameObject.GetComponent<SplineAnimate>().Container = collision.gameObject.GetComponent<SplineContainer>();
                    gameObject.GetComponent<SplineAnimate>().Restart(true);
                    gameObject.GetComponent<SplineAnimate>().Play();
                }
            }

            if (collision.CompareTag("Runway"))
            {
                if (onApproach == true && landed == false)
                {
                    gameObject.GetComponent<SplineAnimate>().StartOffset = 0;
                    gameObject.GetComponent<SplineAnimate>().Container = collision.gameObject.GetComponent<SplineContainer>();
                    gameObject.GetComponent<SplineAnimate>().Restart(true);
                    //gameObject.GetComponent<SplineAnimate>().Play();
                    //gameObject.GetComponent<SplineAnimate>().Play();

                    landed = true;
                }
            }

            if (collision.CompareTag("RunwayEnd"))
            {
                if (landed)
                {
                    Debug.Log("Destroy");
                    MouseCont.pauseG -= PausePlane;
                    MouseCont.startG -= StartPlane;
                    uiCanvas.GetComponent<UIManager>().PlaneLanded();
                    Destroy(gameObject);
                }
            }
        }
        **/

        if(collision.CompareTag("ApproachLong"))
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
        }
    }
}
