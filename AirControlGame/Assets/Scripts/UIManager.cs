using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public int planesLanded = 0;
    public GameObject score;
    public GameObject WindDir;
    public GameObject timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaneLanded()
    {
        planesLanded++;
        score.GetComponent<TextMeshProUGUI>().text = "Planes Landed: " + planesLanded;
    }

    public void UpdateWindDirection(string newText)
    {
        WindDir.GetComponent<TextMeshProUGUI>().text = newText;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UpdateTimer(string time)
    {
        timer.SetActive(true);
        timer.GetComponent<TextMeshProUGUI>().text = time;
    }
}
