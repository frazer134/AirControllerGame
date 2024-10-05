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

    public float acceleration = 0.1f;
    public float speed = 0.1f;
    public float scale = 1f;
    public bool TakeoffStarted = false;

    public AudioClip TakeoffClip;
    public AudioClip onGoalClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

        if(inAir == false && gameObject.GetComponent<SplineGen>().nSpline != null)
        {
            if(gameObject.GetComponent<MoveAlongSpline>().GetDistance() == 0 && gameObject.GetComponent<MoveAlongSpline>().moving == false)
            {
                inAir = true;
                gameObject.GetComponent<SplineGen>().inAir = true;
                gameObject.GetComponent<SplineGen>().paused = false;
                radioFlash.SetActive(true);
                StartCoroutine("InAirFlash");
            }
        }

        if (inAir == false)
        {
            speed = speed + acceleration;
            if(speed > 2f)
            {
                speed = 2f;
            }
            gameObject.GetComponent<MoveAlongSpline>().speed = speed;
        }
        else
        {
            gameObject.GetComponent<MoveAlongSpline>().speed = 2f;
        }

        if(TakeoffStarted == true)
        {
            scale = scale + (acceleration/20);
            if(scale < 1f)
            {
                scale = 1f;
            }
            if(scale > 2f)
            {
                scale = 2f;
                TakeoffStarted = false;
            }
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(2, 2, 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var name = collision.gameObject.name;

        if (name == "NorthGoal" && goal == 0)
        {
            //Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
                gameObject.GetComponent<AudioSource>().clip = onGoalClip;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(name == "EastGoal" && goal == 1)
        {
            //Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
                gameObject.GetComponent<AudioSource>().clip = onGoalClip;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(name == "SouthGoal" && goal == 2)
        {
            //Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
                gameObject.GetComponent<AudioSource>().clip = onGoalClip;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(name == "WestGoal" && goal == 3)
        {
            //Debug.Log("Takeoff Score");
            //gameObject.GetComponent<SplineGen>().PlaneDestroyed();
            onGoal = true;
            if (inAir == true)
            {
                radioFlash.SetActive(true);
                gameObject.GetComponent<AudioSource>().clip = onGoalClip;
                gameObject.GetComponent<AudioSource>().Play();
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

    public void PlayTakeoffSound()
    {
        gameObject.GetComponent<AudioSource>().clip = TakeoffClip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    IEnumerator InAirFlash()
    {
        yield return new WaitForSeconds(1);
        radioFlash.SetActive(false);
    }
}
