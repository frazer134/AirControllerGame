using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 1f;
    public float distancePercentage = 0f;

    public float splineLength;

    public bool moving = false;
    public bool calc = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(calc == true)
        {
            SplineUpadte(spline);
            calc = false;
        }
        if (moving == true)
        {
            distancePercentage += speed * Time.deltaTime / splineLength;

            Vector3 currentPos = spline.EvaluatePosition(distancePercentage);
            transform.position = currentPos;

            Vector3 nextpos = spline.EvaluatePosition(distancePercentage + 0.05f);
            Vector3 direction = nextpos - currentPos;

            if (distancePercentage > 0.9f)
            {
                distancePercentage = 0f;
                moving = false;
                spline = null;
            }
            else
            {
                Quaternion rightToforward = Quaternion.Euler(0f, -90f, 0f);
                Quaternion forwardToRight = Quaternion.LookRotation(direction, -Vector3.up);

                transform.rotation = (forwardToRight * rightToforward);
            }
        }
        }
    

    public float GetDistance()
    {
        return distancePercentage;
    }

    public void SplineUpadte(SplineContainer nSpline)
    {
        spline = nSpline;
        splineLength = spline.CalculateLength();
        distancePercentage= 0f;
        moving = true;
        calc = true;
    }


}
