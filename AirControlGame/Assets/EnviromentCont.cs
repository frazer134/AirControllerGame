using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentCont : MonoBehaviour
{
    public List<GameObject> runwayList = new List<GameObject>();
    public float minTime = 10f;
    public float maxTime = 20f;
    public float timePassed = 0f;

    public UIManager uiCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timePassed < maxTime)
        {
            if(timePassed > minTime)
            {
                if(Random.Range(0,10) == 7)
                {
                    TurnRunways();
                    timePassed = 0f;
                }
            }
            timePassed = timePassed + Time.deltaTime;
        }
        else
        {
            TurnRunways();
            timePassed = 0f;
        }
    }

    public void TurnRunways()
    {
        int windDir = Random.Range(0, 4);
        if (windDir == 0)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().northRot;
                uiCanvas.UpdateWindDirection("Wind N");
            }
        }
        else if (windDir == 1)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().southRot;
                uiCanvas.UpdateWindDirection("Wind S");
            }
        }
        else if (windDir == 2)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().eastRot;
                uiCanvas.UpdateWindDirection("Wind E");
            }
        }
        else
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().westRot;
                uiCanvas.UpdateWindDirection("Wind W");

            }
        }
    }
}
