using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{
    public List<Item> item = new List<Item>();
	public List<Drag> drag;
	public GameObject inventory;
	
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InventoryActive();
    }
	
	public void InventoryActive()
	{
		if(Input.GetKeyDown(KeyCode.I))
		{
			if(inventory.activeSelf)
			{
				InventoryDisable();
			}
			else
			{
				InventoryEnable();
			}
		}
	}
	public void InventoryDisable()
	{
		foreach (Drag drag in drag)
			drag.RemoveCell();
		inventory.SetActive(false);
		
	}
	public void InventoryEnable()
	{
		inventory.SetActive(true);
		
		foreach(Drag drag in drag)
			drag.RemoveCell();
	
		for(int i = 0; i < item.Count; i++)
		{
			drag[i].item = item[i];
			drag[i].image.sprite = Resources.Load<Sprite>(item[i].pathSprite);
			drag[i].ownerItem = "myItem";
		}
	}
	
	public void RemoveItem(Drag drag)
	{
		print("remove");
	}
	public void UseItem(Drag drag)
	{
		print("use");
	}
}
