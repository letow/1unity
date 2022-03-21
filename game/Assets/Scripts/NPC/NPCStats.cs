using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    Animator anim;
	public bool dead;
    public float health;
    public Transform[] element;
    public Transform item;

    CapsuleCollider capsuleCollider;

    public delegate void ImDead(GameObject who);
    public static event ImDead whoDead;

    void Start()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void TakeAwayHealth(int takeAway)
    {
        health -= takeAway;
        if (health <= 0)
            Die();
    }
    public void Die()
    {
        dead = true;
		anim.SetBool("Death", true);
        capsuleCollider.enabled = false;
        whoDead?.Invoke(who: gameObject);
    }
}
