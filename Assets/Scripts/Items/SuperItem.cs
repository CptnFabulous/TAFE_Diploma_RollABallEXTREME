using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperItem : Item
{
    public ParticleSystem pickupEffect;
    
    public override void Collect()
    {
        Instantiate(pickupEffect.gameObject, transform.position, Quaternion.identity);
        base.Collect();
    }
}
