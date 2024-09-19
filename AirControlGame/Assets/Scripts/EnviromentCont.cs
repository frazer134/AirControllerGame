using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentCont : MonoBehaviour
{
    public List<GameObject> runwayList = new List<GameObject>();
    public float minTime = 10f;
    public float maxTime = 20f;
    public float timePassed = 0f;

    float flashDelay = 1f;
    public float flashTime = 30f;

    public float passedFlashTime = 0f;
    public bool uiBool = true;

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
            StartCoroutine(FlashUI(windDir, "Wind: N"));
        }
        if (windDir == 1)
        {
            StartCoroutine(FlashUI(windDir, "Wind: E"));
        }
        if (windDir == 2)
        {
            StartCoroutine(FlashUI(windDir, "Wind: S"));
        }
        if (windDir == 3)
        {
            StartCoroutine(FlashUI(windDir, "Wind: W"));
        }
    }

    IEnumerator FlashUI(float newRot, string uiText)
    {
        var startTime = Time.time;
        //var timeN = Time.time;
        //var uiBool = true;

        while(passedFlashTime < flashTime)
        {
            Debug.Log(Time.time);
            Debug.Log(startTime);
            if(Time.time - startTime > flashDelay)
            {
                Debug.Log("If Loop");
                startTime = Time.time;
                uiBool = !uiBool;
                uiCanvas.WindDir.SetActive(uiBool);
            }
            //timeN = Time.time;
            passedFlashTime = passedFlashTime + Time.deltaTime;
        }

        yield return new WaitForSeconds(3f);

        DelayTurn(newRot, uiText);
    }

    public void DelayTurn(float newRot1, string uiText1)
    {
        if (newRot1 == 0)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().northRot;
                uiCanvas.UpdateWindDirection("Wind N");
            }
        }
        if(newRot1 ==1)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().eastRot;
                uiCanvas.UpdateWindDirection("Wind E");
            }
        }
        if(newRot1 == 2)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().southRot;
                uiCanvas.UpdateWindDirection("Wind S");
            }
        }
        if(newRot1 == 3)
        {
            foreach (GameObject runway in runwayList)
            {
                runway.transform.rotation = runway.GetComponent<RunwayData>().westRot;
                uiCanvas.UpdateWindDirection("Wind W");
            }
        }
    }
}
