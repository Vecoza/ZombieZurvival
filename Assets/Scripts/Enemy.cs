using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = Unity.Mathematics.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHP = 100, currentHP;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navAgent;

    [SerializeField] private List<GameObject> spawnItems;

    [SerializeField] internal bool isDead;
    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP-= damageAmount;
        if (currentHP <= 0)
        {
            int randomValue = UnityEngine.Random.Range(0, 2);

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
                var temp = Instantiate(spawnItems[UnityEngine.Random.Range(0, 6)],
                    transform.position + new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity);
                temp.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            
            isDead = true;
            //dead sound
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            
            //hurt sounde
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }
}