using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBang : MonoBehaviour
{

    public Transform targetInteract;
    public Vector3 targetPos;

    Vector3 direction;
    Quaternion lookRotation;

    public float speed;
    public float distance;

    public AudioClip missileBang;
    public AudioClip missileCast;
    AudioSource audioSource;

    public int damage = 50;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject Caster = GameObject.Find("Player");
        HeroSpells heroSpells = Caster.GetComponent<HeroSpells>();
        targetInteract = heroSpells.targetInteract;
        targetPos = heroSpells.targetInteract.position + Vector3.up;
        audioSource = GameObject.Find("Magic").GetComponent<AudioSource>();
        audioSource.PlayOneShot(missileCast, 0.17f);
        print("Взяли");
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = targetInteract.position + Vector3.up;
        distance = Vector3.Distance(transform.position, targetPos);
        speed = 6;
        transform.LookAt(targetPos);


        if (distance > 0.1f)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
	
	void OnTriggerEnter()
	{
		print("Бам нахуй !!");
        audioSource.PlayOneShot(missileBang, 0.08f);
        targetInteract.GetComponent<NPCStats>().TakeAwayHealth(damage);
		Destroy(gameObject);
	}
}
