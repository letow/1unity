using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeroStats : MonoBehaviour
{
	Animator anim;
	Menu menu;
	Inventory inventoryMain;
    public float health;
    public float maxHealth;
	public Image imgHealth;

	public float mana;
	public float maxMana;
	public Image imgMana;

	public int level;
	public float exp;
	public float curExp;
	public Image imgExp;

	public int strenght;
	public float multiplierStrenght;

	public int intelligence;
	public float multiplierIntelligence;

	public GameObject deathScreen;

	private void Start()
	{
		menu = GetComponent<Menu>();
		exp = 100 * level;
		anim = GetComponent<Animator>();
		inventoryMain = GameObject.Find("Inventory").transform.GetComponent<Inventory>();
		health = inventoryMain.health;
		mana = inventoryMain.mana;
		curExp = inventoryMain.curExp;
		InterfaceUpdate();
	}

    private void Update()
    {
		health += Time.deltaTime / 3;
		health = Mathf.Clamp(health, 0, maxHealth);
		inventoryMain.health = health;
		mana += Time.deltaTime / 3;
		mana = Mathf.Clamp(mana, 0, maxHealth);
		inventoryMain.mana = mana;
		InterfaceUpdate();
	}

    public void AddHealth(int add)
    {
        health += add;

        health = Mathf.Clamp(health, 0, maxHealth);
		InterfaceUpdate();
    }
	
	public void AddExp(int add)
	{
		curExp += add;
		if (curExp == exp)
		{
			level++;
			exp = 100 * level;
			curExp = 0;
			strenght += 3;
			intelligence += 3;
			CalculateStats();
		}
		else if (curExp > exp)
		{
			curExp -= exp;
			level++;
			exp = 100 * level;
			strenght += 3;
			intelligence += 3;
			CalculateStats();
		}
		InterfaceUpdate();
	}
	
	public void InterfaceUpdate()
	{
		float timeHealth = health / maxHealth * 100;
		imgHealth.fillAmount = timeHealth / 100;

		float timeMana = mana / maxMana * 100;
		imgMana.fillAmount = timeMana / 100;

		float timeExp = curExp / exp * 100;
		imgExp.fillAmount = timeExp / 100;
	}

	public void CalculateStats()
	{
		maxHealth = strenght * multiplierStrenght;
	}
	public void TakeAwayHealth(int takeAway)
	{
		health -= takeAway;
		if (health <= 0)
		{
			print("You died");
			Death();
		}
	}

	public bool TakeAwayMana(int takeAway)
	{
		float startMana = mana;
		mana -= takeAway;
		if (mana < 0)
		{
			print("Недостаточно маны");
			mana = startMana;
			return false;
		}
		else return true;
	}

	public void Death()
	{
		deathScreen.SetActive(true);
		anim.SetBool("Death", true);
	}
	
	public void Restart()
	{
		menu.Restart();
	}
	
	public void MainMenu()
	{
		menu.MainMenu();
	}
}
