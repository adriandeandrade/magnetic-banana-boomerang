using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : Trap
{
    Animator anim;
    [Tooltip("The amount of damage")]
    [SerializeField] private float damage;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void Activate()
    {
        InitializeTimer();
        anim.SetTrigger("activate");
        ApplyDamage();
    }

    public override void Deactivate()
    {
        anim.SetTrigger("deactivate");
        active = false;
    }

    private void ApplyDamage()
    {
        List<GameObject> otherObjects = DetectObjectsWithinRadius(transform, range);

        if (otherObjects.Count > 0)
        {
            foreach (GameObject otherObject in otherObjects)
            {
                Vector2 dir = otherObject.transform.position - transform.position;
                IDamageable damageable = otherObject.GetComponent<IDamageable>();

                if (damageable != null)
                {

                    damageable.TakeDamage(damage, dir);
                }
            }
        }
    }
}
