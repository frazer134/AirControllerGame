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

        gameObject.GetComponent<SplineAnimate>().enabled = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Plane"))
        {

        }
    }

    public void PlaneDestroyed()
    {
        MouseCont.pauseG -= PausePlane;
        MouseCont.startG -= StartPlane;
        uiCanvas.GetComponent<UIManager>().PlaneLanded();
        Destroy(gameObject);
;    }
}
