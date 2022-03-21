using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	
	public string nameItem;
	public string typeItem;
	public int price;
	public int addHealth;
	[Multiline(10)]
	public string description;
	public string pathSprite;
	public string pathPrefab;
	public bool isStackable;
	public Vector3 position;
	public Vector3 rotation;
	public int damage;
	public Spell spell;
}
