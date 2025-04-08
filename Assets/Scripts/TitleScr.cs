using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScr : MonoBehaviour
{
    public Transform startMarker; // 
    public Transform endMarker;   // 
    public float speed = 150f;    // 
    private float startTime;      // 
    private float journeyLength;  //
    private bool shoudstart = false;
    public bool TitleGo = false;
    void OnEnable()
    {
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        shoudstart = true;
        goBack();
        TitleGo = true;
    }

    public void goBack()
    {
        transform.position = startMarker.position;
        startTime = Time.time; // 
    }
    // Update is called once per frame
    void Update()
    {
        if (shoudstart)
        {
            // 
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;

            // 
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

            // 
            if (fracJourney >= 1.0f)
            {
                Debug.Log("WorkaetNe?");
                journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
                shoudstart = false;
                TitleGo = false;
                gameObject.SetActive(false);
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
