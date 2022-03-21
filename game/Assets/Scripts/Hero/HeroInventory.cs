using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroInventory : MonoBehaviour
{
	HeroStats heroStats;
	Animator anim;
	
	public int money;
	public Text moneyAmount;
    public List<Item> food = new List<Item>();
	public List<Item> weapon = new List<Item>();

	public int typeOutput;
	
	public Drag mainWeapon;
	public GameObject weaponInHand;
	public Transform rHand;
	public Transform Spine;
	public bool equip;
	
	public Vector3 positionSpine;
	public Vector3 rotationSpine;
	public Vector3 positionRHand;
	public Vector3 rotationRHand;
	public List<Drag> drag;
	public GameObject inventory;
	public Inventory inventoryMain;

	public GameObject cell;
	public Transform cellParent;

	public AudioClip useApple;
	public AudioClip drawingSword;
	AudioSource audioSource;
	
	public GameObject descriptionObj;
	public Text descriptionItem;
	[HideInInspector]
	public bool Trade;
	public Dealer dealer;
	
    void Start()
    {
		typeOutput = 1;
		heroStats = GetComponent<HeroStats>();
		anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		moneyAmount.text = money.ToString();
		inventoryMain = GameObject.Find("Inventory").transform.GetComponent<Inventory>();
		food = inventoryMain.food;
		weapon = inventoryMain.weapon;
		money = inventoryMain.money;
		weaponInHand = inventoryMain.weaponInHand;
		if(inventoryMain.prevLocation == "Taverna")
		{
			transform.position = GameObject.Find("Taverna").transform.GetChild(0).transform.position;
			transform.rotation = GameObject.Find("Taverna").transform.GetChild(0).transform.rotation;
		}
    }

    // Update is called once per frame
    void Update()
    {
        InventoryActive();

		if(weaponInHand != null)
		{
			if(equip && Input.GetKeyDown(KeyCode.X))
			{
				if(anim.GetBool("Equip"))
				{
					anim.SetBool("Equip", false);
					anim.SetFloat("TypeEquip", 1);
					equip = false;
				}
				else
				{
					anim.SetBool("Equip", true);
					anim.SetFloat("TypeEquip", 0);
					equip = false;
				}
				Invoke("Equip", 3);
			}
		}

    }
	
	public void InventoryActive()
	{
		if(Input.GetKeyDown(KeyCode.I) && !Trade)
		{
			if(inventory.activeSelf)
			{
				InventoryDisable();
			}
			else
			{
				InventoryEnabled();
			}
		}
	}
	public void InventoryDisable()
	{
		foreach (Drag drag in drag)
			Destroy(drag.gameObject);
		drag.Clear();
		inventory.SetActive(false);
		descriptionObj.SetActive(false);
	}
	public void InventoryEnabled()
	{
		inventory.SetActive(true);

		foreach (Drag drag in drag)
			Destroy(drag.gameObject);
		drag.Clear();



        if (typeOutput == 1)
        {
			for (int i = 0; i < food.Count; i++)
			{
				GameObject newCell = Instantiate(cell);
				newCell.transform.SetParent(cellParent, false);
				drag.Add(newCell.GetComponent<Drag>());
			}
			for (int i = 0; i < food.Count; i++)
			{
				Item it = food[i];
				for (int j = 0; j < food.Count; j++)
				{
					if (drag[j].ownerItem != "")
					{
						if (food[i].isStackable)
						{
							if (drag[j].item.nameItem == it.nameItem)
							{
								drag[j].countItem++;
								if(drag[j].countItem > 1) drag[j].count.text = drag[j].countItem.ToString();
								break;
							}
						}
						else
						{
							continue;
						}
					}
					else
					{
						drag[j].item = it;
						drag[j].image.sprite = Resources.Load<Sprite>(it.pathSprite);
						drag[j].ownerItem = "myItem";
						drag[j].countItem++;
						if(drag[j].countItem > 1) drag[j].count.text = "" + drag[j].countItem;
						drag[j].heroInventory = this;
						drag[j].descriptionObj = descriptionObj;
						drag[j].descriptionItem = descriptionItem;
						break;
					}
				}
			}
		}
        else if (typeOutput == 2)
        {
			for (int i = 0; i < weapon.Count; i++)
			{
				GameObject newCell = Instantiate(cell);
				newCell.transform.SetParent(cellParent, false);
				drag.Add(newCell.GetComponent<Drag>());
			}
			for (int i = 0; i < weapon.Count; i++)
			{
				Item it = weapon[i];
				for (int j = 0; j < weapon.Count; j++)
				{
					if (drag[j].ownerItem != "")
					{
						if (weapon[i].isStackable)
						{
							if (drag[j].item.nameItem == it.nameItem)
							{
								drag[j].countItem++;
								if(drag[j].countItem > 1) drag[j].count.text = drag[j].countItem.ToString();
								break;
							}
						}
						else
						{
							continue;
						}
					}
					else
					{
						drag[j].item = it;
						drag[j].image.sprite = Resources.Load<Sprite>(it.pathSprite);
						drag[j].ownerItem = "myItem";
						drag[j].countItem++;
						if(drag[j].countItem > 1) drag[j].count.text = "" + drag[j].countItem;
						drag[j].heroInventory = this;
						drag[j].descriptionObj = descriptionObj;
						drag[j].descriptionItem = descriptionItem;
						break;
					}
				}
			}
		}

		

        for (int i = drag.Count - 1;  i >= 0; i --)
        {
			if (drag[i].ownerItem == "")
			{
				Destroy(drag[i].gameObject);
				drag.RemoveAt(i);
			}
        }
	}
	
	public void OutputFood()
    {
		typeOutput = 1;
		InventoryEnabled();
    }
	public void OutputWeapon()
	{
		typeOutput = 2;
		InventoryEnabled();
	}

	public void RemoveItem(Drag drag)
	{
		Item it = drag.item;
		if (it.typeItem != "QuestFood")
		{
			GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
			newObj.transform.position = transform.position + transform.forward + transform.up;
			food.Remove(it);
			weapon.Remove(it);
		}
		InventoryEnabled();
	}
	public void UseItem(Drag drag)
	{
		Item it = drag.item;

		if (it.typeItem == "Food")
		{
			heroStats.AddHealth(drag.item.addHealth);
			food.Remove(it);
			if(it.nameItem == "Green apple" || it.nameItem == "Red apple")
			{
				audioSource.PlayOneShot(useApple, 0.4f);
			}
		}
		else if (it.typeItem == "Weapon")
		{
			if(drag.ownerItem == "myItem")
			{
				if(weaponInHand == null)
				{
					GameObject newObj = Instantiate<GameObject>(Resources.Load<GameObject>(it.pathPrefab));
					newObj.transform.SetParent(Spine);
					newObj.transform.localPosition = positionSpine;
					newObj.transform.localRotation = Quaternion.Euler(rotationSpine);
					newObj.GetComponent<Rigidbody>().isKinematic = true;
					newObj.GetComponent<MeshCollider>().enabled = false;
					weaponInHand = newObj;
					
					mainWeapon.item = it;
					mainWeapon.image.sprite = Resources.Load<Sprite>(it.pathSprite);
					mainWeapon.ownerItem = "myWeapon";
					mainWeapon.heroInventory = this;
					
					weapon.Remove(it);
				}
			}
			
			else if (drag.ownerItem == "myWeapon")
			{
				weapon.Add(drag.item);
				
				Destroy(weaponInHand);
				weaponInHand = null;
				
				mainWeapon.item = null;
				mainWeapon.image.sprite = mainWeapon.defaultSprite;
				mainWeapon.ownerItem = "";
				mainWeapon.heroInventory = null;
			}
		}
		
		InventoryEnabled();
	}
	
	public void Buy(Drag drag)
	{
		Item it = drag.item;
		
		if(it.price <= money)
		{

			if(it.typeItem == "Food")
			{
				food.Add(it);
				dealer.item.Remove(it);
				
				InventoryEnabled();
				dealer.enabledInventory();
			}
			else if(it.typeItem == "Weapon")
			{
				weapon.Add(it);
				dealer.item.Remove(it);
				
				InventoryEnabled();
				dealer.enabledInventory();
			}
		}
		money -= it.price;
		moneyAmount.text = money.ToString();
	}

	public void Sell(Drag drag)
	{
		Item it = drag.item;

		if(it.typeItem == "Food" || it.typeItem == "QuestFood")
		{
			food.Remove(it);
			dealer.item.Add(it);
			
			InventoryEnabled();
			dealer.enabledInventory();
		}
		else if(it.typeItem == "Weapon")
		{
			weapon.Remove(it);
			dealer.item.Add(it);
			
			InventoryEnabled();
			dealer.enabledInventory();
		}	
		money += it.price;
		moneyAmount.text = money.ToString();
	}
	
	public void TakeSword()
	{
		if(anim.GetBool("Equip"))
		{
			weaponInHand.transform.SetParent(rHand);
			weaponInHand.transform.localPosition = positionRHand;
			weaponInHand.transform.localRotation = Quaternion.Euler(rotationRHand);
			audioSource.PlayOneShot(drawingSword, 0.2f);
		}
		else
		{
			weaponInHand.transform.SetParent(Spine);
			weaponInHand.transform.localPosition = positionSpine;
			weaponInHand.transform.localRotation = Quaternion.Euler(rotationSpine);
		}
	}
	
	public void Equip()
	{
		equip = true;
	}
}
