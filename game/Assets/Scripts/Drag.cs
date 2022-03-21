using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	public HeroInventory heroInventory;
	public Item item;
	public string ownerItem;
	public int countItem;
	
	public Image image;
	public Sprite defaultSprite;
	public Text count;
	
	public Text descriptionText;
	public GameObject descriptionObj;
	public Text descriptionItem;
	
	Image img;
	
	public void Start()
	{
		img = GetComponent<Image>();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(ownerItem == "myItem")
		{
			img.color = new Color(1f,1f,1f,60f/255);
			descriptionObj.SetActive(true);
			descriptionItem.text = item.description;
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		if(ownerItem == "myItem")
		{
			img.color = new Color(1f,1f,1f,100f/255);
			descriptionObj.SetActive(false);
			descriptionItem.text = "";
		}
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if(ownerItem != "")
		{
			if(eventData.button == PointerEventData.InputButton.Right)
			{
				if(ownerItem == "myItem")
				{
					heroInventory.RemoveItem(this);
				}
				if(descriptionObj) descriptionObj.SetActive(false);
			}
			else if (eventData.button == PointerEventData.InputButton.Left)
			{
				if(ownerItem == "myItem" || ownerItem == "myWeapon")
				{
					if(!heroInventory.Trade)
					{
						heroInventory.UseItem(this);
					}
					else
					{
						heroInventory.Sell(this);
					}
				}
				else if(ownerItem == "DealerItem")
				{
					heroInventory.Buy(this);
				}
				if(descriptionObj) descriptionObj.SetActive(false);
			}
		}
	}
	
}
