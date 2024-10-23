using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject uiText;

    [SerializeField] private string text1;
    [SerializeField] private string text2;
    [SerializeField] private string text3;
    [SerializeField] private string text4;
    [SerializeField] private string text5;
    [SerializeField] private string text6;

    [SerializeField] private GameObject Runway2;

    [SerializeField] private TakeoffPlaneSpawner spawnT;
    [SerializeField] private LandingPlaneSpawn spawnL1;
    [SerializeField] private LandingPlaneSpawn spawnL2;

    [SerializeField] private int currentState = 0;

    GameObject plane1;



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
            var text1 = uiText.transform.Find("InstText");
            text1.GetComponent<TextMeshProUGUI>().text = text3;
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

    }

    private void TerrainCollision()
    {

    }

    private void WindDir()
    {

    }
}
