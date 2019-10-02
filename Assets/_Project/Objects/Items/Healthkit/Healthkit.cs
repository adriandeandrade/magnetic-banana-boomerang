using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthkit : ItemBehavior
{
    [SerializeField] private float healAmount = 2f; 

    public override void OnItemReachDestination()
    {
        player.AddHealth(healAmount);
        Destroy(gameObject);
    }
}
