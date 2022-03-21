using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public List<Transform> wayPoint;
    public int curWayPoint;
	public NPCStats npcStats;
    Animator anim;
    public float speed;
    public int damage;
    AudioSource audioSource;
    public AudioClip hitSound;
    NavMeshAgent agent;


    Transform player;
    Transform target;

    public Transform head;
    public int visible;
    public int angleView;

    float distance;
    float angle;

    Vector3 direction;
    Quaternion lookRotation;
    Quaternion look;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = player.Find("Sound").GetComponent<AudioSource>();
    }

    private void Update()
    {
		if(!npcStats.dead)
		{
			FindTargetRayCast();
			if (target == null)
			{
				anim.SetBool("Attack", false);
				Patrol();
			}
			else if (target != null)
				Attack();
		}
		else agent.isStopped = true;
    }

    public void FindTargetRayCast()
    {
        if (target == null)
        {
            distance = Vector3.Distance(head.position, player.position);
            if (distance <= visible)
            {
                look = Quaternion.LookRotation(player.position - head.position);
                angle = Quaternion.Angle(head.rotation, look);

                if (angle <= angleView)
                {
                    RaycastHit hit;
                    Debug.DrawLine(head.position, player.position + Vector3.up * 1.6f);
                    if (Physics.Linecast(head.position, player.position + Vector3.up * 1.6f, out hit) && hit.transform != head && hit.transform != transform)
                    {
                        if (hit.transform == player)
                        {
                            target = player;
                        }
                        //else target = null;
                    }
                    else target = null;
                }
                else target = null;
            }
            else target = null;
        }
        else
        {
            RaycastHit hit;
            Debug.DrawLine(head.position, player.position + Vector3.up * 1.6f);
            if (Physics.Linecast(head.position, player.position + Vector3.up * 1.6f, out hit) && hit.transform != head && hit.transform != transform)
            {
                if (hit.transform == player)
                {
                    target = player;
                }
                else target = null;
            }

        }
    }

    void Patrol()
    {
        if (wayPoint.Count > 1)
        {
            if (wayPoint.Count > curWayPoint)
            {
                agent.SetDestination(wayPoint[curWayPoint].position);
                distance = Vector3.Distance(transform.position, wayPoint[curWayPoint].position);
                if (distance > 2.5f)
                {
                    agent.isStopped = false;
                    anim.SetFloat("Speed", speed);
                    speed += Time.deltaTime * 3;

                }
                else if (distance <= 2.5f && distance >= 1f)
                {
                    direction = (wayPoint[curWayPoint].position - transform.position).normalized;
                    lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
                }
                else
                {
                    curWayPoint++;
                }
            }
            else if (wayPoint.Count == curWayPoint)
            {
                curWayPoint = 0;
            }
        }
        else if (wayPoint.Count == 1)
        {
            agent.SetDestination(wayPoint[0].position);
            distance = Vector3.Distance(transform.position, wayPoint[curWayPoint].position);
            if (distance >= 1.5f)
            {
                agent.isStopped = false;
                anim.SetFloat("Speed", speed);
                speed += Time.deltaTime * 5;
            }
            else
            {
                agent.isStopped = true;
                anim.SetFloat("Speed", speed);
                speed -= Time.deltaTime * 5;

                direction = (wayPoint[0].position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
            }
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", speed);
            speed -= Time.deltaTime * 5;
        }
        speed = Mathf.Clamp(speed, 0, 1);
    }

    public void Attack()
    {
        agent.SetDestination(target.position);
        distance = Vector3.Distance(transform.position, target.position);
        if (distance >= 1.5f)
        {
            agent.isStopped = false;
            anim.SetFloat("Speed", speed);
            anim.SetBool("Attack", false);
            speed += Time.deltaTime * 5;
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", speed);
            speed -= Time.deltaTime * 5;

            direction = (target.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

            anim.SetBool("Attack", true);
        }
        speed = Mathf.Clamp(speed, 0, 1);
    }

    public void Damage()
    {
        target.GetComponent<HeroStats>().TakeAwayHealth(damage);
        audioSource.PlayOneShot(hitSound, 0.075f);
        if (target.GetComponent<HeroStats>().health <= 0)
            target = null;
    }

}
