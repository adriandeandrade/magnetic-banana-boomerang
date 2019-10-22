using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : Trap
{
    Animator anim;
    [Tooltip("The amount of damage")]
    [SerializeField] private float damage;

    bool damageApplied = false;
    float damageDelay = 0.8f;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void Activate()
    {
        if (!active)
        {
            InitializeTimer();
            damageApplied = false;
            anim.SetTrigger("activate");
            //ApplyDamage();
            print("Activating");
            //ActivateSequence();
        }
    }

    protected override void UpdateCallback()
    {
        print("In update " + currentActiveTime);
        if (!damageApplied && activeTime - currentActiveTime > damageDelay)
        {
            ApplyDamage();
            damageApplied = true;
        }
    }

    /*IEnumerable ActivateSequence()
    {
        print("Wait for it ...");
        yield return new WaitForSeconds(1);
        print("Damage!");
        ApplyDamage();
    }*/

    public override void Deactivate()
    {
        print("Deactivating");
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
