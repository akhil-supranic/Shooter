using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : HealthandDamage
{
    public override void Die()
    {
        base.Die();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}
