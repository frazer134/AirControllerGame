using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Splines;

public class TakeoffPlaneSpawner : MonoBehaviour
{

    public List<GameObject> takeoffList = new List<GameObject>();
    public GameObject planeObj;
    public float offset;

    public float timePassed = 0f;
    public float spawnTime =10f;

    public GameObject uiCanvas;
    public float timer = 60f;
    public int maxplanes = 5;

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
            if (timePassed < spawnTime)
            {
                timePassed = timePassed + Time.deltaTime;
            }
            else
            {
                int goal = Random.Range(0, 4);
                CreatePlane(goal);
                timePassed = 0f;
            }
        }

        if (gameP == false)
        {
            if (takeoffList.Count >= maxplanes)
            {
                timer = timer - Time.deltaTime;
                int TimeINT = (int)timer;
                uiCanvas.GetComponent<UIManager>().UpdateTimer(TimeINT.ToString());

                if (timer <= 0)
                {
                    var camera = GameObject.Find("Main Camera");
                    camera.GetComponent<MouseCont>().StopGame();
                    camera.GetComponent<CameraCont>().EndGame(takeoffList[takeoffList.Count - 1]);
                }
            }
            else
            {
                timer = 30f;
                uiCanvas.GetComponent<UIManager>().TurnOffTimerUI();
            }
        }
    }

    public void CreatePlane(int goal)
    {
        var nPlane = Instantiate(planeObj);
        nPlane.GetComponent<SplineGen>().PausePlane();
        nPlane.GetComponent<SplineGen>().enabled = false;
        nPlane.GetComponent<SplineGen>().uiCanvas = uiCanvas;
        takeoffList.Add(nPlane);
        UpdateDisplay();

        nPlane.GetComponent<TakeoffCont>().goal = goal;
    }

    public void UpdateDisplay()
    {
        for(int i =0; i<= takeoffList.Count-1; i++)
        {
            var pos = gameObject.transform.position;
            Vector3 posOffset = new Vector3(1, i*offset, 2);
            pos = pos + posOffset;
            takeoffList[i].transform.position = pos;
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

    public GameObject TutorialSpwan(int goal)
    {
        var nPlane = Instantiate(planeObj);
        nPlane.GetComponent<SplineGen>().PausePlane();
        nPlane.GetComponent<SplineGen>().enabled = false;
        nPlane.GetComponent<SplineGen>().uiCanvas = uiCanvas;
        takeoffList.Add(nPlane);
        UpdateDisplay();

        nPlane.GetComponent<TakeoffCont>().goal = goal;

        return nPlane;
    }
}
