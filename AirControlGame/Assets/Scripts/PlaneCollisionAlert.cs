using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollisionAlert : MonoBehaviour
{

    public GameObject planeMaster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 off1 = new Vector3(0, 0.5f, 0);
        RaycastHit2D hit1 = Physics2D.Raycast(planeMaster.transform.position + off1, (planeMaster.transform.position + off1) + planeMaster.transform.right, 60f, LayerMask.GetMask("DetectLayer"));
        RaycastHit2D hit2 = Physics2D.Raycast(planeMaster.transform.position - off1, (planeMaster.transform.position - off1) + planeMaster.transform.right, 60f, LayerMask.GetMask("DetectLayer"));

        Debug.DrawLine(gameObject.transform.position + off1, (gameObject.transform.position + off1) + gameObject.transform.right * 5f);
        Debug.DrawLine(gameObject.transform.position - off1, (gameObject.transform.position - off1) + gameObject.transform.right * 5f);

        //Debug.Log("Hit1: " + hit1.collider.gameObject + "    Hit2: " + hit2.collider.gameObject);

        if (hit1.collider != null)
        {
            if (hit1.collider.gameObject.CompareTag("PlanePCollider"))
            {
                if (hit1.collider.gameObject != gameObject)
                {
                    //hit1.collider.gameObject.SetActive(false);
                    Debug.Log("Hit1: " + hit1.collider.gameObject);
                    if (hit1.collider.gameObject.GetComponent<PlaneCollisionAlert>().planeMaster.GetComponent<SplineGen>().inAir == true)
                    {
                        planeMaster.GetComponent<SplineGen>().AlarmOn();
                        hit1.collider.gameObject.GetComponent<PlaneCollisionAlert>().planeMaster.GetComponent<SplineGen>().AlarmOn();
                    }
                    else
                    {
                        planeMaster.GetComponent<SplineGen>().AlarmOff();
                        hit1.collider.gameObject.GetComponent<PlaneCollisionAlert>().planeMaster.GetComponent<SplineGen>().AlarmOff();
                    }
                }
            }
        }
        else if (hit2.collider != null)
        {
            if (hit2.collider.gameObject.CompareTag("PlanePCollider"))
            {
                if (hit2.collider.gameObject != gameObject)
                {
                    //hit2.collider.gameObject.SetActive(false);
                    Debug.Log("Hit2 :" + hit2.collider.gameObject);
                    if (hit2.collider.gameObject.GetComponent<PlaneCollisionAlert>().planeMaster.GetComponent<SplineGen>().inAir == true)
                    {
                        planeMaster.GetComponent<SplineGen>().AlarmOn();
                        hit2.collider.gameObject.GetComponent<PlaneCollisionAlert>().planeMaster.GetComponent<SplineGen>().AlarmOn();
                    }
                    else
                    {
                        planeMaster.GetComponent<SplineGen>().AlarmOff();
                        hit2.collider.gameObject.GetComponent<PlaneCollisionAlert>().planeMaster.GetComponent<SplineGen>().AlarmOff();
                    }
                }
            }
        }
        else
        {
            planeMaster.GetComponent<SplineGen>().AlarmOff();
        }
    }
}
