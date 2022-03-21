using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DontDestroy : MonoBehaviour 
{
    [HideInInspector]
    public string objectID;

    //static Inventory inventoryMain;
    void Awake()
    {
        objectID = name + "(0,0,0)";

    }
    public void Start()
    {
        DonotDestroy();
    }
    public void DonotDestroy()
    {
        for (int i = 0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++)
        {
            if (Object.FindObjectsOfType<DontDestroy>()[i] != this)
            {
                if (Object.FindObjectsOfType<DontDestroy>()[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
}
