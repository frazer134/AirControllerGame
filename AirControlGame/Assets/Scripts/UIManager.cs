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

    public GameObject wrongRunway;

    private IEnumerator coroutine;
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

    public void WrongRunwayHit(Vector3 gPos)
    {

        float offsetY = gPos.y + 2f;

        Vector3 offsetPos = new Vector3(gPos.x, offsetY, gPos.z);
        
        Vector2 CanvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetComponent<RectTransform>(), screenPoint, null, out CanvasPos);

        wrongRunway.transform.localPosition = CanvasPos;
        wrongRunway.SetActive(true);

        coroutine = TurnOffUI(wrongRunway, 2f);
        StartCoroutine(coroutine);
    }

    private IEnumerator TurnOffUI(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
