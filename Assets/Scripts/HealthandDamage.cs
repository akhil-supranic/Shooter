using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthandDamage : MonoBehaviour
{
    public float health;
    public float damage;

    public virtual void TakeDamage(float damage)
    {
        health-=damage;
        Debug.Log(health);
        if(health<=0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + "Dead");
        Destroy(gameObject);
    }
}
