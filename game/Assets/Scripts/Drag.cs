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
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			heroInventory.RemoveItem(this);
		}
		else if (eventData.button == PointerEventData.InputButton.Left)
		{
			heroInventory.UseItem(this);
		}
	}
	
	public void RemoveCell()
	{
		item = null;
		image.sprite = null;
		countItem = 0;
		count.text = "";
		//descriptionText.text = "";
		ownerItem = "";
	}
}
