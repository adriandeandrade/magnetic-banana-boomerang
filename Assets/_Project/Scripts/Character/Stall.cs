using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class Stall : MonoBehaviour
{
    [Header("Stall Setup")]
    [Tooltip("How long the character will be stalled for.")]
    [SerializeField] private float stallTime;

    BaseEnemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<BaseEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyStall()
    {
        print("Stalled");
        enemy.SetState(EnemyStates.IDLE);
    }

    public void RemoveStall()
    {
        enemy.SetState(EnemyStates.MOVING);
    }
}
