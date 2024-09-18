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

    public GameObject oldSpline = null;

    public float forwardSpeed = 1f;
    public bool paused = false;

    public bool LongLand = false;

    public GameObject uiCanvas;

    public bool TakeoffPlane = false;

    public bool inAir = false;

    public bool started = false;

    //public delegate void PauseGame();
    //public static event PauseGame pauseG;


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
        //Debug.Log("Plane Stopped");
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
            if (started == true && collision.gameObject.GetComponent<SplineGen>().started == true)
            {
                if (inAir == true && collision.gameObject.GetComponent<SplineGen>().inAir == true)
                {
                    Debug.Log("Plane Collision");
                    var camera = GameObject.Find("Main Camera");
                    camera.GetComponent<MouseCont>().StopGame();
                    camera.GetComponent<CameraCont>().EndGame(gameObject);
                }
                else if (inAir == false && collision.gameObject.GetComponent<SplineGen>().inAir == false)
                {
                    Debug.Log("Plane Collision");
                    var camera = GameObject.Find("MainCamera");
                    camera.GetComponent<MouseCont>().StopGame();
                    camera.GetComponent<CameraCont>().EndGame(gameObject);
                }
            }
        }
    }

    public void PlaneDestroyed()
    {
        MouseCont.pauseG -= PausePlane;
        MouseCont.startG -= StartPlane;
        uiCanvas.GetComponent<UIManager>().PlaneLanded();
        Destroy(nSpline.gameObject);
        if(gameObject.GetComponent<SplineAnimate>().Container != null)
        {
            Destroy(gameObject.GetComponent<SplineAnimate>().Container.gameObject);
        }
        Destroy(gameObject);
;    }
}
