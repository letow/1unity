using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
    public GameObject task;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !task.activeSelf)
        {
            task.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && task.activeSelf)
        {
            task.SetActive(false);
        }
    }
}
