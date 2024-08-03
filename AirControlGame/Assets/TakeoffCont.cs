using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TakeoffCont : MonoBehaviour
{

    public SplineContainer takeoffS1;
    public SplineContainer takeoffS2;

    public bool takeoff = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void StartTakeoff()
    {
        if(takeoffS1 != null && takeoffS2 != null)
        {
            gameObject.GetComponent<SplineAnimate>().Container = takeoffS1;
            gameObject.GetComponent<SplineAnimate>().Play();
        }
    }
}
