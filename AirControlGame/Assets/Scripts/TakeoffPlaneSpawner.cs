using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
    }

    public void CreatePlane(int goal)
    {
        var nPlane = Instantiate(planeObj);
        nPlane.GetComponent<SplineAnimate>().enabled = false;
        nPlane.GetComponent<SplineGen>().PausePlane();
        nPlane.GetComponent<SplineGen>().enabled = false;
        nPlane.GetComponent<SplineGen>().uiCanvas = uiCanvas;
        takeoffList.Add(nPlane);
        UpdateDisplay();

        nPlane.GetComponent<TakeoffCont>().goal = goal;
    }

    /**
    public void AddDisplay(GameObject nPlane)
    {
        for(int i =0; i <= takeoffList.Count; i++)
        {
            var pos = gameObject.transform.position;
            Vector3 posOffset = new Vector3(i*offset,1,2);
            pos = pos + posOffset;
            nPlane.transform.position = pos;
;        }
    }
    **/

    public void UpdateDisplay()
    {
        for(int i =0; i<= takeoffList.Count-1; i++)
        {
            var pos = gameObject.transform.position;
            Vector3 posOffset = new Vector3(i*offset, 1, 2);
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
}
