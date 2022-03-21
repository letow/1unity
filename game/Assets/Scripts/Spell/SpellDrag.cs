using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellDrag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	public HeroSpells heroSpells;
	public Spell spell;
	
	public string ownerSpell;
	public Sprite defaultSprite;
	public Text descriptionText;

	void Start()
	{
		GameObject paladin = GameObject.Find("Player");
		HeroSpells paladinSpells = paladin.GetComponent<HeroSpells>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{

	}
	public void OnPointerExit(PointerEventData eventData)
	{

	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (ownerSpell == "mySpell")
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				heroSpells.Cast(this.spell);
			}
			else if (eventData.button == PointerEventData.InputButton.Right)
			{
				//heroInventory.UseItem(this);
			}
		}
	}
}
