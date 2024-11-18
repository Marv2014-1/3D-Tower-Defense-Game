using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    public float blockChance = 0.5f;
    public float blockCooldown = 2f;
    public int shieldStrength = 100;
    public float blockDamageReduction = 1.0f;

    private bool isBlocking = false;
    private float nextBlockTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsBlocking()
    {
        if (Time.time >= nextBlockTime)
        {
            isBlocking = Random.value <= blockChance;
            nextBlockTime = Time.time + blockCooldown;
        }
        return isBlocking;
    }

    public int CalculateDamageAfterBlock(int incomingDamage)
    {
        return isBlocking ? Mathf.CeilToInt(incomingDamage * blockDamageReduction) : incomingDamage;
    }
}
