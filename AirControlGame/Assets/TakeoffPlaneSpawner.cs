using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TakeoffPlaneSpawner : MonoBehaviour
{

    public List<GameObject> takeoffList = new List<GameObject>();
    public GameObject planeObj;
    public float offset;

    public float timePassed = 0f;
    public float spawnTime =10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timePassed < spawnTime)
        {
            timePassed = timePassed + Time.deltaTime;
        }
        else
        {
            CreatePlane();
            timePassed = 0f;
        }
    }

    public void CreatePlane()
    {
        var nPlane = Instantiate(planeObj);
        nPlane.GetComponent<SplineGen>().PausePlane();
        takeoffList.Add(planeObj);
        UpdateDisplay(nPlane);
    }

    public void UpdateDisplay(GameObject nPlane)
    {
        for(int i =0; i <= takeoffList.Count; i++)
        {
            var pos = gameObject.transform.position;
            Vector3 posOffset = new Vector3(i*offset,0,0);
            pos = pos + posOffset;
            nPlane.transform.position = pos;
;        }
    }
}
