using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
	NavMeshAgent agent;
	AudioSource audioSource;
	AudioSource audioSourceFootsteps;
	Animator anim;
	Menu menu;
	Dialogs dialogs;
	HeroInventory heroInventory;
	HeroSpells heroSpells;
	HeroStats heroStats;

	public float speed;
	public float maxSpeed;
	public float distance;

	public Vector3 target;
	public Transform targetInteract;

	public string act;
	public AudioClip hitSoundSkeleton;
	public AudioClip hitSoundGolem;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		heroInventory = GetComponent<HeroInventory>();
		heroSpells = GetComponent<HeroSpells>();
		heroStats = GetComponent<HeroStats>(); ;
		dialogs = GetComponent<Dialogs>();
		menu = GetComponent<Menu>();
		audioSource = transform.Find("Sound").GetComponent<AudioSource>();
		audioSourceFootsteps = transform.Find("footsteps").GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey(KeyCode.LeftAlt))
		{
			maxSpeed += 0.05f;
			maxSpeed = Mathf.Clamp(maxSpeed, 1, 2);
		}
		else
		{
			maxSpeed -= 0.05f;
			maxSpeed = Mathf.Clamp(maxSpeed, 1, 2);
		}
		
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 100f))
			{
				ClickUpdate(hit);
			}
		}

		switch (act)
		{
			case "Move": Move(); break;
			case "Attack": Attack(); break;
			case "Cast": Cast(); break;
			case "Dealer": Dealer(); break;
			case "Priest": Priest(); break;
			case "Building": Building(); break;
			case "": AnimOff(); break;
		}
		if(speed > 1.5f)
		{
			audioSourceFootsteps.pitch = 2;
		}
		else
		{
			audioSourceFootsteps.pitch = 1;
		}

    }
	void ClickUpdate(RaycastHit hit)
	{
		if (hit.transform.tag == "Ground")
		{
			target = hit.point;
			targetInteract = null;
			act = "Move";
			heroSpells.castReady = false;
		}
		else if (hit.transform.tag == "Item")
		{
			targetInteract = null;
			TakeItem(hit);
			heroSpells.castReady = false;
		}
		else if (heroSpells.castReady && hit.transform.tag == "Enemy")
		{
			print("Клик по врагу, установили акт");
			targetInteract = hit.transform;
			act = "Cast";
		}
		else if (hit.transform.tag == "Enemy")
		{
			heroSpells.castReady = false;
			if (heroInventory.weaponInHand != null)
			{
				if (anim.GetBool("Equip"))
				{
					targetInteract = hit.transform;
					act = "Attack";
				}
				else
				{
					anim.SetBool("Equip", true);
					anim.SetFloat("TypeEquip", 0);
				}
			}
		}
		else if (hit.transform.tag == "Dealer")
		{
			targetInteract = hit.transform;
			act = "Dealer";
		}
		else if (hit.transform.tag == "Priest")
		{
			targetInteract = hit.transform;
			act = "Priest";
		}
		else if (hit.transform.tag == "Building")
		{
			targetInteract = hit.transform.GetChild(0).transform;
			act = "Building";
		}
	}
	
	void Move()
	{
		distance = Vector3.Distance(transform.position, target);


		if (distance > 0.6f)
		{
			agent.SetDestination(target);
			agent.isStopped = false;
			if (!audioSourceFootsteps.isPlaying) audioSourceFootsteps.Play();
			speed += 1.0f * Time.deltaTime;
			anim.SetBool("Walk", true);
			anim.SetBool("Attack", false);
			
			if(distance >= 1f)
			{
				speed += 3.5f * Time.deltaTime;
			}
			else if(distance < 1f)
			{
				if(speed > distance)
				{
					speed -= 3* Time.deltaTime;
				}
				else
				{
					speed += 2 * Time.deltaTime;
				}
			}
		}
		else if (distance <= 0.6f)
		{
			speed -= 3 * Time.deltaTime;
			target = transform.position;
			
			if (speed <= 0.2f)
			{
				anim.SetBool("Attack", false);
				anim.SetBool("Walk", false);
				agent.isStopped = true;
				audioSourceFootsteps.Stop();
				act = "";
				speed = 0;
			}
		}
		anim.SetFloat("Speed", speed);
		speed = Mathf.Clamp(speed, 0, maxSpeed);
	}

	void Attack()
	{
		distance = Vector3.Distance(transform.position, targetInteract.position);
		anim.SetFloat("Speed", speed);
		speed = Mathf.Clamp(speed, 0, 1);

		if (distance > 2.4f)
		{
			agent.SetDestination(targetInteract.position);
			agent.isStopped = false;
			if (!audioSourceFootsteps.isPlaying) audioSourceFootsteps.Play();
			speed += 2 * Time.deltaTime;
			anim.SetBool("Walk", true);
			anim.SetBool("Attack", false);
		}
		else if (distance <= 3f)
		{
			speed -= 4 * Time.deltaTime;

			Vector3 direction = (targetInteract.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
			if (speed <= 0.2f)
			{
				anim.SetBool("Walk", false);
				anim.SetBool("Attack", true);
				agent.isStopped = true;
				audioSourceFootsteps.Stop();
			}
		}

	}
	
	void Dealer()
	{
		distance = Vector3.Distance(transform.position, targetInteract.Find("TradePoint").position);
		anim.SetFloat("Speed", speed);
		speed = Mathf.Clamp(speed, 0, 1);
		
		if (distance > 0.95f)
		{
			agent.SetDestination(targetInteract.Find("TradePoint").position);
			agent.isStopped = false;
			if (!audioSourceFootsteps.isPlaying) audioSourceFootsteps.Play();
			speed += 2 * Time.deltaTime;
			anim.SetBool("Walk", true);
			anim.SetBool("Attack", false);
		}
		else
		{
			Dealer dealer = targetInteract.GetComponent<Dealer>();
			speed = 0;
			dealer.heroInventory = heroInventory;
			heroInventory.Trade = true;
			heroInventory.dealer = dealer;
			dealer.enabled = true;
			agent.isStopped = true;
			audioSourceFootsteps.Stop();
			act = "";
			dialogs.Dialog(targetInteract.name);
		}
	}

	void Priest()
	{
		distance = Vector3.Distance(transform.position, targetInteract.Find("TalkPoint").position);
		anim.SetFloat("Speed", speed);
		speed = Mathf.Clamp(speed, 0, maxSpeed);
		
		if (distance > 1.8f)
		{
			agent.SetDestination(targetInteract.Find("TalkPoint").position);
			agent.isStopped = false;
			if (!audioSourceFootsteps.isPlaying) audioSourceFootsteps.Play();
			speed += 2 * Time.deltaTime;
			anim.SetBool("Walk", true);
			anim.SetBool("Attack", false);
		}
		else
		{
			speed = 0;
			agent.isStopped = true;
			audioSourceFootsteps.Stop();
			act = "";
			dialogs.Dialog(targetInteract.name);
		}
	}
	
	void AnimOff()
	{
		anim.SetBool("Walk", false);
		anim.SetBool("Attack", false);
	}
	
	void TakeItem(RaycastHit hit)
	{
		distance = Vector3.Distance(transform.position + transform.up, hit.transform.position);
		Item it = hit.transform.GetComponent<Item>();

		if (distance < 2)
		{
			if (it.typeItem == "Food" || it.typeItem == "QuestFood")
			{
				heroInventory.food.Add(hit.transform.GetComponent<Item>());
				heroInventory.moneyAmount.text = heroInventory.money.ToString();
				hit.transform.gameObject.SetActive(false);
				if(hit.transform.GetComponent<Item>().nameItem == "Cabbage")
				{
					heroInventory.inventoryMain.cabbageAmount++;
				}
				heroInventory.inventoryMain.transform.parent = hit.transform;
				DontDestroyOnLoad(hit.transform);
				heroInventory.inventoryMain.GetComponent<DontDestroy>().DonotDestroy();
				if(heroInventory.inventory.activeSelf) heroInventory.InventoryEnabled();
			}
			else if (it.typeItem == "Weapon")
			{
				heroInventory.weapon.Add(hit.transform.GetComponent<Item>());
				heroInventory.moneyAmount.text = heroInventory.money.ToString();
				hit.transform.gameObject.SetActive(false);
				heroInventory.inventoryMain.transform.parent = hit.transform;
				DontDestroyOnLoad(hit.transform);
				heroInventory.inventoryMain.GetComponent<DontDestroy>().DonotDestroy();
				if(heroInventory.inventory.activeSelf) heroInventory.InventoryEnabled();
			}
			else if (it.typeItem == "Gold")
			{
				heroInventory.money += it.price;
				heroInventory.moneyAmount.text = heroInventory.money.ToString();
				Destroy(hit.transform.gameObject);
				if(heroInventory.inventory.activeSelf) heroInventory.InventoryEnabled();
			}
			else if (it.typeItem == "Scroll")
			{
				heroSpells.spells.Add(it.spell);
				Destroy(hit.transform.gameObject);
				if (heroSpells.spellBook.activeSelf) heroSpells.SpellBookEnable();
			}
		}
		else
		{
			target = hit.point + Vector3.right/2 + Vector3.forward/2;
			targetInteract = null;
			act = "Move";
			print("Далеко");
		}

	}

	void Cast()
	{
		if (heroStats.TakeAwayMana(heroSpells.spells[0].manacost)) // 0 - какой в списке спелов спелл ыыыыыыыыы
		{
			agent.isStopped = true;
			Vector3 direction = (targetInteract.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 500);
			heroSpells.castReady = true;
			anim.SetBool("Walk", false);
			anim.SetBool("Attack", false);
			anim.SetBool("Casting", true);
			heroSpells.targetInteract = targetInteract;
			act = "";
		}
	}
	
	void Building()
	{

		distance = Vector3.Distance(transform.position, targetInteract.position);
		anim.SetFloat("Speed", speed);
		speed = Mathf.Clamp(speed, 0, maxSpeed);
		
		if (distance > 0.7f)
		{
			agent.SetDestination(targetInteract.position);
			agent.isStopped = false;
			if (!audioSourceFootsteps.isPlaying) audioSourceFootsteps.Play();
			speed += 2 * Time.deltaTime;
			anim.SetBool("Walk", true);
		}
		else
		{
			agent.isStopped = true;
			audioSourceFootsteps.Stop();
			speed -= 2 * Time.deltaTime;
			anim.SetBool("Walk", false);
			name = targetInteract.parent.name;
			targetInteract = null;
			act = "";
			menu.Door(name);
		}
	}
	
	public void Damage()
    {
		targetInteract.GetComponent<NPCStats>().TakeAwayHealth(heroInventory.mainWeapon.item.damage);

		if(targetInteract.name != "Golem")
		{
			audioSource.PlayOneShot(hitSoundSkeleton, 0.1f);
		}
		else
		{
			audioSource.PlayOneShot(hitSoundGolem, 0.1f);
		}

		if(targetInteract.GetComponent<NPCStats>().health <= 0)
		{
			act = "";
		}
    }

}
