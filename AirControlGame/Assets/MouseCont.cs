using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Splines;

public class MouseCont : MonoBehaviour
{
    public GameObject currentPlane;
    public List<Vector3> splinePoints = new List<Vector3>();

    public float pTime = 1f;
    public float time = 0f;

    public Material pathMat;
    public Mesh pathMesh;

    public delegate void PauseGame();
    public static event PauseGame pauseG;

    public delegate void StartGame();
    public static event StartGame startG;

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
            if (hitP.collider.CompareTag("Plane"))
            {
                currentPlane = hitP.collider.gameObject;
                splinePoints.Add(currentPlane.transform.position);
                if(pauseG!=null)
                {
                    pauseG();
                }
            }
        }

        if (currentPlane != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (time > pTime)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        //Debug.Log("Hit: " + hit.collider.gameObject.name);
                        var hitPoint = new Vector3(hit.point.x, hit.point.y, -2);
                        splinePoints.Add(hitPoint);
                        //Instantiate(point, hitPoint, Quaternion.identity);
                        time = 0f;
                    }
                }
                else
                {
                    time = time + Time.deltaTime;
                }
            }
            else
            {
                currentPlane.GetComponent<SplineGen>().GenPlanePath(splinePoints, pathMesh);
                currentPlane = null;
                splinePoints.Clear();
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(startG!=null)
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
    }

    public void PauseTest()
    {
        Debug.Log("Game Paused");
    }
}
