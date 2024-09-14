using System.Collections;
using System.Collections.Generic;
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

    public SplineContainer startSpline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time > minTime)
        {
            if(Random.Range(0,10) == 7)
            {
                SpawnPlane();
                //var nPlane = Instantiate(plane);
                //nPlane.GetComponent<SplineAnimate>().Container = startSpline;
                //nPlane.GetComponent<SplineAnimate>().Play();
                //time = 0;
            }
            else if(time > maxTime)
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

    private void SpawnPlane()
    {
        // 1 in 5 chance of spawning long land plane
        if (Random.Range(1, 5) == 3)
        {
            var nPlane = Instantiate(plane);
            nPlane.GetComponent<SplineGen>().uiCanvas= canvasUI;
            nPlane.GetComponent<SplineGen>().inAir = true;
            nPlane.GetComponent<LandCont>().longLand = true;
            nPlane.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = longP;
            nPlane.transform.GetChild(0).transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            nPlane.GetComponent<SplineAnimate>().Container = startSpline;
            nPlane.GetComponent<SplineAnimate>().Play();
            time = 0;
        }
        else
        {
            var nPlane = Instantiate(plane);
            nPlane.GetComponent<SplineGen>().uiCanvas = canvasUI;
            nPlane.GetComponent<SplineGen>().inAir = true;
            nPlane.GetComponent<LandCont>().longLand = false;
            nPlane.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shortP;
            nPlane.transform.GetChild(0).transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            nPlane.GetComponent<SplineAnimate>().Container = startSpline;
            nPlane.GetComponent<SplineAnimate>().Play();
            time = 0;
        }
    }
}
