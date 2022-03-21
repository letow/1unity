using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfEvangelion : MonoBehaviour
{
    Menu menu;
    void Awake()
    {
        menu = GetComponent<Menu>();
        NPCStats.whoDead += this.CheckWhoDead;
    }

    public void CheckWhoDead(GameObject who)
    {
        if (who.name == "Golem")
        {
            menu.EndOfEvangelion();
        }
    }
}
