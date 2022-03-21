using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public List<Item> food = new List<Item>();
    public List<Item> weapon = new List<Item>();
    public int money;
    public float health;
    public float mana;
    public float curExp;
    public bool firstTime;
    public bool cabbageTrigger;
    public int cabbageAmount;
    public string prevLocation;
    public bool curTask;
    public bool priest;
    public bool scroll;
    public bool soldCabbage;
    public GameObject weaponInHand;

    void Start()
    {
        mana = 100f;
        health = 100f;
        firstTime = true;
    }

    void Update()
    {
        if(cabbageAmount == 3)
        {
            cabbageTrigger = true;
        }
        else
        {
            cabbageTrigger = false;
        }
    }

    
}
