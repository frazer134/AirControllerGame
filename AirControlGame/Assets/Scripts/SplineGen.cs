using Assets.Scenes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SplineGen : MonoBehaviour
{
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

    public Quaternion offsetRot;
    public GameObject spriteHolder;

    //public delegate void PauseGame();
    //public static event PauseGame pauseG;


    // Start is called before the first frame update
    void Start()
    {
        MouseCont.pauseG += PausePlane;
        MouseCont.startG += StartPlane;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == false)
        {
            if (gameObject.GetComponent<MoveAlongSpline>().moving == false && gameObject.GetComponent<MoveAlongSpline>().GetDistance() == 0f)
            {
                transform.position = transform.position + ((transform.right * forwardSpeed) * Time.deltaTime);
            }
        }

    }

    public void GenPlanePath(List<Vector3> planePath, List<Quaternion> planeRot,  Mesh defaultMesh)
    {

        nSpline = SplineMaker.SplineGenerator(planePath, planeRot, defaultMesh);

        gameObject.GetComponent<MoveAlongSpline>().SplineUpadte(nSpline);
        gameObject.GetComponent<MoveAlongSpline>().moving = true;

        splineGenerated = true;

        SplineCleanUp(nSpline);

        /**
        for (int k = 0; k < nSpline.Spline.Knots.ToArray().Length; k++)
        {
            Quaternion knotRot = nSpline.Spline.Knots.ToArray()[k].Rotation;
            Quaternion newRot = new Quaternion(0, 0, 0, 0);

            var newKnot = nSpline.Spline.Knots.ToArray()[k];
            newRot.eulerAngles = new Vector3(knotRot.x, 270, 180);
            newKnot.Rotation= newRot;

            nSpline.Spline.SetKnot(k, newKnot);
        }
        **/
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
        gameObject.GetComponent<MoveAlongSpline>().moving = false;
        paused= true;
    }

    public void StartPlane()
    {
        if (gameObject.GetComponent<MoveAlongSpline>().spline != null)
        {
            gameObject.GetComponent<MoveAlongSpline>().moving = true;
        }
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
        gameObject.GetComponent<MoveAlongSpline>().moving = false;
        uiCanvas.GetComponent<UIManager>().PlaneLanded();
        Destroy(nSpline.gameObject);
        Destroy(gameObject);
    }

}
