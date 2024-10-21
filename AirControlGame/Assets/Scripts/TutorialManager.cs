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

    int currentState = 0;

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
    }

    public void NextStep()
    {
        switch(currentState)
        {
            case 0: LongLand();
                break;
            case 1: Takeoff();
                break;
            case 2: TakeoffTimer();
                break;
            case 3: TerrainCollision();
                break;
            case 4: WindDir();
                break;
        }
    }

    private void LongLand()
    {
        //Step #2
        currentState = currentState + 1;
        uiText.SetActive(false);
        plane1 = spawnL1.TutorialSpawnner(false);
    }

    private void Takeoff()
    {

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
