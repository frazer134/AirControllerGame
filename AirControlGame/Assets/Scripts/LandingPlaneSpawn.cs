using Assets.Scenes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class LandingPlaneSpawn : MonoBehaviour
{
    public GameObject plane;

    public float minTime;
    public float maxTime;
    public float time;

    public Sprite shortP;
    public Sprite longP;

    public GameObject canvasUI;

    public bool gameP = false;

    // Start is called before the first frame update
    void Start()
    {
        MouseCont.pauseG += PauseSpawn;
        MouseCont.startG += StartSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameP == false)
        {
            if (time > minTime)
            {
                if (Random.Range(0, 10) == 7)
                {
                    SpawnPlane();
                    //var nPlane = Instantiate(plane);
                    //nPlane.GetComponent<SplineAnimate>().Container = startSpline;
                    //nPlane.GetComponent<SplineAnimate>().Play();
                    //time = 0;
                }
                else if (time > maxTime)
                {
                    SpawnPlane();
                    //var nPlane = Instantiate(plane);
                    //nPlane.GetComponent<SplineAnimate>().Container = startSpline;
                    //nPlane.GetComponent<SplineAnimate>().Play();
                    //time = 0;
                }
                else
                {
                    time = time + Time.deltaTime;
                }
            }
            else
            {
                time = time + Time.deltaTime;
            }
        }
    }

    private void SpawnPlane()
    {
        Quaternion knotRot = new Quaternion();
        knotRot.eulerAngles = new Vector3(0, 0, -90);
        

        // 1 in 5 chance of spawning long land plane
        if (Random.Range(1, 5) == 3)
        {
            var nPlane = Instantiate(plane, gameObject.transform.position, knotRot);
            nPlane.GetComponent<SplineGen>().uiCanvas= canvasUI;
            nPlane.GetComponent<SplineGen>().inAir = true;
            nPlane.GetComponent<SplineGen>().started = true;
            nPlane.GetComponent<LandCont>().longLand = true;
            nPlane.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = longP;
            time = 0;
        }
        else
        {
            var nPlane = Instantiate(plane, gameObject.transform.position, knotRot);
            nPlane.GetComponent<SplineGen>().uiCanvas = canvasUI;
            nPlane.GetComponent<SplineGen>().inAir = true;
            nPlane.GetComponent<SplineGen>().started = true;
            nPlane.GetComponent<LandCont>().longLand = false;
            nPlane.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shortP;
            time = 0;
        }
    }
    public void PauseSpawn()
    {
        gameP = true;
    }

    public void StartSpawn()
    {
        gameP = false;
    }
}
