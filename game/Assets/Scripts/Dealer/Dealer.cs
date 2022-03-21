using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
	public List<Item> item = new List<Item>();
	
	[HideInInspector]
	public HeroInventory heroInventory;
	
	public List<Drag> drag;
	public GameObject inventory;
	public GameObject cell;
	public Transform cellParent;


    void OnEnable()
    {
        enabledInventory();
    }
	
	public void enabledInventory()
	{

        inventory.SetActive(true);
		heroInventory.InventoryEnabled();
		
		foreach (Drag drag in drag)
			Destroy(drag.gameObject);
		drag.Clear();

		for (int i = 0; i < item.Count; i++)
		{
			GameObject newCell = Instantiate(cell);
			newCell.transform.SetParent(cellParent, false);
			drag.Add(newCell.GetComponent<Drag>());
		}
		for (int i = 0; i < item.Count; i++)
		{
			Item it = item[i];
			
			drag[i].item = it;
			drag[i].image.sprite = Resources.Load<Sprite>(it.pathSprite);
			drag[i].ownerItem = "DealerItem";
			drag[i].countItem++;
			drag[i].heroInventory = heroInventory;
			
		}
	}
	
	void OnDisable()
	{
		foreach (Drag drag in drag)
			Destroy(drag.gameObject);
		drag.Clear();
		
		inventory.SetActive(false);
	}
	
	public void Close()
	{
		heroInventory.InventoryDisable();
		heroInventory.Trade = false;
		enabled = false;
		heroInventory.dealer = null;
	}
}
