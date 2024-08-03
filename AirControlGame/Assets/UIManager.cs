using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public int planesLanded = 0;
    public GameObject score;
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
}
