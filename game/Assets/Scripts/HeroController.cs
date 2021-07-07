using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
	NavMeshAgent agent;
	Animator anim;
	
	public float speed;
	public float distance;
	public Vector3 target;

	public string act;
	
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
		}

    }
	void ClickUpdate(RaycastHit hit)
	{
		if (hit.transform.tag == "Ground")
		{
			target = hit.point;
			act = "Move";
		}
	}
	void Move()
	{
		distance = Vector3.Distance(transform.position, target);
		anim.SetFloat("Speed", speed);
		speed = Mathf.Clamp(speed, 0, 1);

		if (distance > 0.5f)
		{
			agent.SetDestination(target);
			agent.isStopped = false;
			speed += 2 * Time.deltaTime;
			anim.SetBool("Walk", true);
		}
		else if (distance <= 0.5f)
		{
			speed -= 4 * Time.deltaTime;

			if (speed <= 0.2f)
			{
				anim.SetBool("Walk", false);
				agent.isStopped = true;
				act = "";
			}
		}

	}
}
