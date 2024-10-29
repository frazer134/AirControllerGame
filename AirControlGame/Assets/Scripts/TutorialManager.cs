using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject uiText;

    [SerializeField] private string text1;
    [SerializeField] private string text2;
    [SerializeField] private string text3;
    [SerializeField] private string text4;
    [SerializeField] private string text5;
    [SerializeField] private string text6;
    [SerializeField] private string text7;

    [SerializeField] private GameObject Runway2;

    [SerializeField] private TakeoffPlaneSpawner spawnT;
    [SerializeField] private LandingPlaneSpawn spawnL1;
    [SerializeField] private LandingPlaneSpawn spawnL2;

    [SerializeField] private int currentState = 0;

    GameObject plane1;

    [SerializeField] private GameObject timerFlash;
    [SerializeField] private GameObject flipCol;
    [SerializeField] private GameObject TerrainCol;

    [SerializeField] private EnviromentCont enviro;



    // Start is called before the first frame update
    void Start()
    {
        // Step #1
        uiText.SetActive(true);
        var text = uiText.transform.Find("InstText");
        text.GetComponent<TextMeshProUGUI>().text = text1;
    }

    // Update is called once per frame
    void Update()
    {
        //Step #3
        if(currentState == 1 && plane1 == null)
        {
            uiText.SetActive(true);
            var text = uiText.transform.Find("InstText");
            text.GetComponent<TextMeshProUGUI>().text = text2;
        }

        //Step #5
        if(currentState == 2 && plane1 == null)
        {
            uiText.SetActive(true);
            var text = uiText.transform.Find("InstText");
            text.GetComponent<TextMeshProUGUI>().text = text3;
        }

        //Step #7
        if(currentState == 3 && plane1 == null)
        {
            uiText.SetActive(true);
            var text = uiText.transform.Find("InstText");
            text.GetComponent<TextMeshProUGUI>().text = text4;
        }

        //Step #9
        if(currentState == 4 && spawnT.takeoffList.Count == 4)
        {
            timerFlash.SetActive(false);
            Destroy(plane1);
            foreach(GameObject p in spawnT.takeoffList)
            {
                Destroy(p);
            }
            uiText.SetActive(true);
            var text = uiText.transform.Find("InstText");
            text.GetComponent<TextMeshProUGUI>().text = text5;
        }

        //Step #11
        if(currentState ==5 && Vector3.Distance(plane1.transform.position, flipCol.transform.position) < 12f)
        {
            plane1.GetComponent<SplineGen>().PausePlane();
            uiText.SetActive(true);
            var text = uiText.transform.Find("InstText");
            text.GetComponent<TextMeshProUGUI>().text = text6;
        }

        if (currentState == 6 && plane1 == null)
        {
            uiText.SetActive(true);
            var text = uiText.transform.Find("InstText");
            text.GetComponent<TextMeshProUGUI>().text = text7;

            var button = uiText.transform.Find("Button");
            var butText = button.transform.Find("Text (TMP)");
            butText.GetComponent<TextMeshProUGUI>().text = "Main Menu";
        }
    }

    public void NextStep()
    {
        switch(currentState)
        {
            case 0: LandPlane();
                break;
            case 1: LongLand();
                break;
            case 2: Takeoff();
                break;
            case 3: TakeoffTimer();
                break;
            case 4: TerrainCollision();
                break;
            case 5: WindDir();
                break;
            case 6: Completed();
                break;
        }
    }

    private void LandPlane()
    {
        //Step #2
        currentState = currentState + 1;
        uiText.SetActive(false);
        plane1 = spawnL1.TutorialSpawnner(false);
    }

    private void LongLand()
    {
        //Step #4
        currentState = currentState + 1;
        uiText.SetActive(false);
        plane1 = spawnL1.TutorialSpawnner(true);
    }

    private void Takeoff()
    {
        //Step #6
        currentState = currentState + 1;
        uiText.SetActive(false);
        plane1 = spawnT.TutorialSpwan(1);
        Runway2.SetActive(true);
    }

    private void TakeoffTimer()
    {
        //Step #8
        currentState = currentState + 1;
        uiText.SetActive(false);
        plane1 = spawnT.TutorialSpwan(1);
        var p2 = spawnT.TutorialSpwan(3);
        var p3 = spawnT.TutorialSpwan(2);
        var p4 = spawnT.TutorialSpwan(2);
        var p5 = spawnT.TutorialSpwan(0);

        timerFlash.SetActive(true);
    }

    //Step #10
    private void TerrainCollision()
    {
        currentState = currentState + 1;
        uiText.SetActive(false);
        TerrainCol.SetActive(true);
        plane1 = spawnL2.TutorialSpawnner(true);
    }

    //Step #12
    private void WindDir()
    {
        currentState = currentState + 1;
        uiText.SetActive(false);
        plane1.GetComponent<SplineGen>().StartPlane();
        enviro.TutorialFlip();
    }

    private void Completed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
