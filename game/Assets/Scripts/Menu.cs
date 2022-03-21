using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	AudioSource audioSource;
	public Animator fade;
	public GameObject player;
	public HeroInventory heroInventory;
	public string prevLocation;
	public GameObject help;
	public GameObject task;
	public GameObject endText;
	public GameObject ending;
	public Vector3 playerStartPos = new Vector3(253.2f, 0.006658554f, 247.0098f);

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
	void Start()
	{
		heroInventory = GetComponent<HeroInventory>();
		task = GameObject.Find("Task");
        if(SceneManager.GetActiveScene().name == "Taverna")
        {
        	Dealer dealer = GameObject.Find("Dealer").transform.GetComponent<Dealer>();
        	heroInventory.dealer = dealer;
        }
	}

    // Update is called once per frame
    void Update()
    {
    	if(SceneManager.GetActiveScene().name != "MainMenu")
    	{
    		Task();
    	}
    }

	public void Task()
    {

        if(heroInventory.inventoryMain.firstTime)
        {
            if(SceneManager.GetActiveScene().name != "Taverna")
            {
                task.transform.GetChild(0).transform.GetComponent<Text>().text = "Visit taverna";
            }
            else
            {
                task.transform.GetChild(0).transform.GetComponent<Text>().text = "Talk to the Dealer";
            }
        }
        else if (!heroInventory.inventoryMain.firstTime && !heroInventory.inventoryMain.cabbageTrigger)
        {
            task.transform.GetChild(0).transform.GetComponent<Text>().text = "Collect three cabbages, sold them to the Dealer and buy a sword";
        }
        else if (heroInventory.inventoryMain.cabbageTrigger && heroInventory.inventoryMain.soldCabbage)
        {
            task.transform.GetChild(0).transform.GetComponent<Text>().text = "Go to a graveyard and deal with skeletons, then go to the Priest";
        }
        else if (heroInventory.inventoryMain.priest)
        {
            task.transform.GetChild(0).transform.GetComponent<Text>().text = "Pick up the Scroll";
        }
        else if (heroInventory.inventoryMain.priest && heroInventory.inventoryMain.scroll)
        {
            task.transform.GetChild(0).transform.GetComponent<Text>().text = "Defeat Golem";
        }
        
    }

    public void Help()
    {
    	help.SetActive(true);
    	audioSource.Play();
    }
    public void HelpClose()
    {
    	help.SetActive(false);
    }
	public void NewGame()
	{
		fade.SetBool("fade", true);
		StartCoroutine(fadeTransition("OpenWorld"));
	}
	
	public void Restart()
	{
		fade.SetBool("fade", true);
		StartCoroutine(fadeTransition(SceneManager.GetActiveScene().name));
	}
	
	public void MainMenu()
	{
		fade.SetBool("fade", true);
		StartCoroutine(fadeTransition("MainMenu"));
	}
	public void Door(string location)
	{
		heroInventory.food = heroInventory.inventoryMain.food;
		heroInventory.weapon = heroInventory.inventoryMain.weapon;
		heroInventory.money = heroInventory.inventoryMain.money;
		heroInventory.weaponInHand = heroInventory.inventoryMain.weaponInHand;
		if(location == "OpenWorld")
		{
			heroInventory.inventoryMain.prevLocation = SceneManager.GetActiveScene().name;
		}
		else
		{
			heroInventory.inventoryMain.prevLocation = "";
		}
		fade.SetBool("fade", true);
		StartCoroutine(fadeTransition(location));
		//player.transform.position = playerStartPos;
	}
	
	IEnumerator fadeTransition(string scene)
	{
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(scene);
	}
	IEnumerator fadeTransitionEnd()
	{
		ending.SetActive(true);
		yield return new WaitForSeconds(5f);
		endText.SetActive(true);
	}
	public void EndOfEvangelion()
	{
		StartCoroutine(fadeTransitionEnd());
	}
	public void Exit()
	{
		Application.Quit();
	}
}
