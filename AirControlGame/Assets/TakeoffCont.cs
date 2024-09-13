using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TakeoffCont : MonoBehaviour
{

    //public SplineContainer takeoffS1;
    //public SplineContainer takeoffS2;

    //public bool takeoff = false;

    public int goal;
    public GameObject Arrow;

    public bool inAir = false;
    public bool onGoal = false;
    public GameObject radioFlash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if(takeoff == true)
        {
            StartTakeoff();
        }

        if (gameObject.GetComponent<SplineAnimate>().Container != null)
        {
            if (gameObject.GetComponent<SplineAnimate>().Container == takeoffS1)
            {
                if (gameObject.GetComponent<SplineAnimate>().NormalizedTime >= 1)
                {
                    gameObject.GetComponent<SplineGen>().nSpline = takeoffS2;
                    gameObject.GetComponent<SplineAnimate>().Container = takeoffS2;
                    gameObject.GetComponent<SplineAnimate>().Play();
                }
            }

            if (gameObject.GetComponent<SplineAnimate>().Container == takeoffS2)
            {
                if (gameObject.GetComponent<SplineAnimate>().NormalizedTime >= 1)
                {
                    takeoff = false;
                }
            }
        }
        **/

        if (goal == 0)
        {
            Arrow.transform.rotation = Quaternion.LookRotation(Camera.main.transform.up, -Camera.main.transform.forward);
        }
        else if (goal == 1)
        {
            Arrow.transform.rotation = Quaternion.LookRotation(Camera.main.transform.right, -Camera.main.transform.forward);
        }
        else if(goal == 2)
        {
            Arrow.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);
        }
        else if(goal == 3)
        {
            Arrow.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.right, -Camera.main.transform.forward);
        }
    }

    /**
    public void StartTakeoff()
    {
        if(takeoffS1 != null && takeoffS2 != null)
        {
            gameObject.GetComponent<SplineAnimate>().Container = takeoffS1;
            gameObject.GetComponent<SplineAnimate>().Play();
        }
        
    }
    **/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var name = collision.gameObject.name;

        if (name == "NorthGoal" && goal == 0)
        {
            Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
            }
        }
        else if(name == "EastGoal" && goal == 1)
        {
            Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
            }
        }
        else if(name == "SouthGoal" && goal == 2)
        {
            Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
            }
        }
        else if(name == "WestGoal" && goal == 3)
        {
            Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "SouthGoal")
        {
            onGoal = false;
        }
    }

    public void PlaneSCore()
    {
        gameObject.GetComponent<SplineGen>().PlaneDestroyed();
    }
}
