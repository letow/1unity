using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoadPointer : MonoBehaviour
{

	public GameObject roadPointer;
	public string currenText;

	public void OnMouseOver()
	{
		roadPointer.transform.GetComponent<Text>().text = currenText;
		roadPointer.SetActive(true);
	}
	public void OnMouseExit()
	{
		roadPointer.transform.GetComponent<Text>().text = "";
		roadPointer.SetActive(false);
	}

}
