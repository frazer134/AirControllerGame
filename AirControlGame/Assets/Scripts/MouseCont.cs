using Assets.Scenes;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class MouseCont : MonoBehaviour
{
    public GameObject currentPlane;
    public List<Vector3> splinePoints = new List<Vector3>();
    public List<Quaternion> splineRot = new List<Quaternion>();

    public float pTime = 1f;
    public float time = 0f;

    public Material pathMat;
    public Mesh pathMesh;

    public GameObject tPlane;
    public GameObject takeoffQueue;
    public bool grabbedPlane = false;
    public float pointDistance = 5f;

    public delegate void PauseGame();
    public static event PauseGame pauseG;

    public delegate void StartGame();
    public static event StartGame startG;

    public SplineContainer tempS;

    // Start is called before the first frame update
    void Start()
    {
        //pauseG = pauseG + PauseTest;
        //pauseG = pauseG + SplineGen.PausePlane;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hitP = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //Debug.Log("Hit: " + hitP.collider.gameObject.name);
            if (hitP.collider != null)
            {
                if (hitP.collider.CompareTag("TakeoffQueue"))
                {
                    if (grabbedPlane == false)
                    {
                        //Debug.Log("Queue Hit");
                        tPlane = takeoffQueue.GetComponent<TakeoffPlaneSpawner>().takeoffList[0];
                        grabbedPlane = true;
                        if (pauseG != null)
                        {
                            pauseG();
                        }
                    }
                }
                else if (hitP.collider.CompareTag("Plane"))
                {
                    if (hitP.collider.gameObject.GetComponent<TakeoffCont>() != null)
                    {
                        if (hitP.collider.gameObject.GetComponent<TakeoffCont>().onGoal == true && hitP.collider.gameObject.GetComponent<TakeoffCont>().inAir == true)
                        {
                            hitP.collider.gameObject.GetComponent<TakeoffCont>().PlaneSCore();
                        }
                        else
                        {
                            if (hitP.collider.gameObject.GetComponent<TakeoffCont>().inAir == true)
                            {
                                //Debug.Log("Plane Hit");
                                currentPlane = hitP.collider.gameObject;
                                splinePoints.Add(currentPlane.transform.position);
                                splineRot.Add(gameObject.transform.rotation);
                                if (pauseG != null)
                                {
                                    pauseG();
                                }
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log("Plane Hit");
                        currentPlane = hitP.collider.gameObject;
                        splinePoints.Add(currentPlane.transform.position);
                        splineRot.Add(gameObject.transform.rotation);
                        if (currentPlane.GetComponent<LandCont>() != null)
                        {
                            currentPlane.GetComponent<LandCont>().ResetInAir();
                        }
                        if (pauseG != null)
                        {
                            pauseG();
                        }
                    }
                }
            }
        }

        if (currentPlane != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //Debug.Log("true");
                if (time > pTime)
                {
                    RaycastHit2D hit = new RaycastHit2D();
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Default"));
                    if (hit.collider != null)
                    {
                        if(splinePoints.Count > 0)
                        {
                            float dist;
                            dist = Vector3.Distance(hit.collider.gameObject.transform.position, splinePoints[splinePoints.Count - 1]);
                            if (dist > pointDistance)
                            {
                                var hitPoint = new Vector3(hit.point.x, hit.point.y, -2);
                                splinePoints.Add(hitPoint);
                                splineRot.Add(gameObject.transform.rotation);
                                DrawPath(splinePoints, splineRot);
                                //Instantiate(point, hitPoint, Quaternion.identity);
                                time = 0f;
                            }
                        }
                    }
                }
                else
                {
                    time = time + Time.deltaTime;
                }
            }
            else
            {
                if (splinePoints.Count > 1)
                {
                    currentPlane.GetComponent<SplineGen>().GenPlanePath(splinePoints, splineRot, pathMesh);
                    currentPlane = null;
                    splinePoints.Clear();
                    splineRot.Clear();
                    if(tempS != null)
                    {
                        Destroy(tempS.gameObject);
                    }
                }
                splinePoints.Clear();
                splineRot.Clear();
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (grabbedPlane == true)
            {
                RaycastHit2D hitP = Physics2D.Raycast(tPlane.transform.position, new Vector3(0,0,1));
                //Debug.Log("Hit: " + hitP.collider.gameObject.name);
                if (hitP.collider.CompareTag("Runway"))
                {
                    var runway = hitP.collider.gameObject.transform.parent;
                    var animator = tPlane.GetComponent<MoveAlongSpline>();
                    var navPoints = new List<Vector3>();
                    navPoints.Add(runway.transform.Find("TakeoffStart").position);
                    navPoints.Add(runway.transform.Find("TakeoffG").position);

                    var rot = new List<Quaternion>();
                    rot.Add(gameObject.transform.rotation);
                    rot.Add(gameObject.transform.rotation);

                    var tSpline = SplineMaker.SplineGenerator(navPoints, rot, pathMesh);

                    animator.SplineUpadte(tSpline);

                    tPlane.GetComponent<SplineGen>().enabled = true;
                    tPlane.GetComponent<SplineGen>().nSpline = tSpline;
                    tPlane.GetComponent<SplineGen>().oldSpline = tSpline.gameObject;
                    tPlane.GetComponent<SplineGen>().started = true;

                    tPlane.GetComponent<MoveAlongSpline>().moving = true;
                    tPlane.GetComponent<TakeoffCont>().TakeoffStarted = true;
                    tPlane.GetComponent<TakeoffCont>().PlayTakeoffSound();
                    grabbedPlane = false;
                    takeoffQueue.GetComponent<TakeoffPlaneSpawner>().takeoffList.Remove(tPlane);
                    takeoffQueue.GetComponent<TakeoffPlaneSpawner>().UpdateDisplay();
                    tPlane = null;
                    currentPlane = null;


                    if (startG != null)
                    {
                        startG();
                    }
                }
                else
                {
                    grabbedPlane = false;
                    tPlane = null;
                    takeoffQueue.GetComponent<TakeoffPlaneSpawner>().UpdateDisplay();
                    if(startG != null)
                    {
                        startG();
                    }
                }
            }
            else if (startG != null)
            {
                startG();
            }
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            RaycastHit2D hit3 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit3.collider != null)
            {
                if(hit3.collider.CompareTag("Plane"))
                {
                    GameObject splineObj = hit3.collider.gameObject.GetComponent<SplineGen>().nSpline.gameObject;
                    Debug.Log(splineObj.name);
                    splineObj.GetComponent<MeshRenderer>().material = pathMat;
                    splineObj.GetComponent<SplineExtrude>().enabled = true;
                    splineObj.GetComponent<MeshRenderer>().enabled = true;
                    splineObj.GetComponent<SplineExtrude>().Rebuild();

                    Time.timeScale = 0.2f;
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            RaycastHit2D hit4 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit4.collider != null)
            {
                if (hit4.collider.CompareTag("Plane"))
                {
                    GameObject splineObj = hit4.collider.gameObject.GetComponent<SplineGen>().nSpline.gameObject;
                    Debug.Log(splineObj.name);
                    splineObj.GetComponent<MeshRenderer>().material = pathMat;
                    //Destroy(splineObj.GetComponent<MeshRenderer>());
                    splineObj.GetComponent<SplineExtrude>().enabled = false;
                    splineObj.GetComponent<MeshRenderer>().enabled = false;

                    Time.timeScale = 1f;
                }
            }
        }

        if (tPlane != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (tPlane != null)
                {
                    if (grabbedPlane == true)
                    {
                        //Debug.Log("Plane Follow Mouse");
                        RaycastHit2D hit5 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        var mouseP = new Vector3(hit5.point.x, hit5.point.y, -1.9f);
                        tPlane.transform.position = mouseP;
                    }
                }
            }
        }
    }

    public void StopGame()
    {
        pauseG?.Invoke();
    }

    public void DrawPath(List<Vector3> pathPoints, List<Quaternion> pathRot)
    {
        if(tempS != null)
        {
            Destroy(tempS.gameObject);
        }
        tempS = SplineMaker.SplineGenerator(pathPoints, pathRot, pathMesh);
        tempS.gameObject.GetComponent<MeshRenderer>().material = pathMat;
        tempS.gameObject.GetComponent<SplineExtrude>().enabled = true;
        tempS.gameObject.GetComponent<MeshRenderer>().enabled = true;
        tempS.gameObject.GetComponent<SplineExtrude>().Rebuild();
        
    }
}
