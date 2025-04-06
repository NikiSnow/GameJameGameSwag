using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialog : MonoBehaviour
{
    public List<GameObject> d = new List<GameObject>();
    private int counter = 0;

    private bool isObject1Active = true;

    void Start()
    {
        d[0].SetActive(true);
        d[1].SetActive(false);
        d[2].SetActive(false);

    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleActiveObjects();
        }
    }

    void ToggleActiveObjects()
    {
        d[counter].SetActive(true);
        counter++;
       
    }
}
