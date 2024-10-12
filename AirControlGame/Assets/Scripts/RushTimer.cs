using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RushTimer : MonoBehaviour
{
    public List<LandingPlaneSpawn> landingList = new List<LandingPlaneSpawn>();
    public UIManager uiManager;
    private bool paused = false;
    private bool landingRush = false;
    private float timePassed = 0f;
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 45f;

    public float rushTime = 90f;

    // Start is called before the first frame update
    void Start()
    {
        MouseCont.pauseG += StopGame;
        MouseCont.startG += StartGame;
        SetSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(paused == false)
        {
            if (landingRush == true)
            {
                if(timePassed > 20f)
                {
                    SetSpawnTime();
                    landingRush = false;
                    timePassed = 0f;
                    uiManager.RushEnded();
                }
                else
                {
                    timePassed = timePassed + Time.deltaTime;
                }
            }
            else
            {
                if (timePassed > rushTime)
                {
                    LandingRush();
                    landingRush = true;
                    timePassed = 0f;
                    uiManager.RushStarted();
                }
                else
                {
                    timePassed = timePassed + Time.deltaTime;
                }
            }
        }
    }

    private void SetSpawnTime()
    {
        foreach (LandingPlaneSpawn S in landingList)
        {
            S.minTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void LandingRush()
    {
        foreach (LandingPlaneSpawn S in landingList)
        {
            S.minTime = Random.Range(minSpawnTime, 30f);
        }
    }

    public void StartGame()
    {
        paused = false;
    }

    public void StopGame()
    {
        paused = true;
    }
}
