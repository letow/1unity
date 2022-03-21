using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public List<Transform> wayPoint;
    public int curWayPoint;
    public Transform sexPosition;

    Animator anim;
    public float speed;

    NavMeshAgent agent;

    public bool sex;

    float distance;
    Vector3 direction;
    Quaternion lookRotation;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!sex)
        {
            Patrol();
        }
        else
        {
            Sex();
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
        anim.SetBool("Sex", false);
        speed = Mathf.Clamp(speed, 0, 1);
    }

    void Sex()
    {
        agent.SetDestination(sexPosition.position);
        distance = Vector3.Distance(transform.position, sexPosition.position);
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

            direction = (sexPosition.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
            anim.SetBool("Sex", true);
        }
        speed = Mathf.Clamp(speed, 0, 1);
    }
}
