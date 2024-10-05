using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCont : MonoBehaviour
{

    //private GameObject zoomObj;
    private Vector3 zoomPos;
    private Quaternion zoomRot;

    public float DelayTime = 0f;
    public float zoomTime = 3f;
    public float cameraZoom = 3f;

    public GameObject uiCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGame(GameObject cPlane)
    {
        zoomPos = new Vector3(cPlane.transform.position.x, cPlane.transform.position.y, -5);
        zoomRot = cPlane.transform.rotation;
        zoomRot.eulerAngles = new Vector3(0,0,zoomRot.z);
        StartCoroutine("MoveCamera");
    }

    IEnumerator MoveCamera()
    {
        float time = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float startCamera = gameObject.GetComponent<Camera>().orthographicSize;
        
        while(time < zoomTime)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, zoomPos, time/zoomTime);
            gameObject.transform.rotation = Quaternion.Lerp(startRot, zoomRot, time/zoomTime);
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(startCamera, cameraZoom, time/zoomTime);
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = zoomPos;
        var button = uiCanvas.transform.GetChild(2);
        button.gameObject.SetActive(true);
    }
}
