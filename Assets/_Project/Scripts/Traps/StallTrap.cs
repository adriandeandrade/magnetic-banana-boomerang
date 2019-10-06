using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class StallTrap : Trap
{
    Animator anim;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Activate()
    {
        InitializeTimer();

        ApplyStall();
    }

    public override void Deactivate()
    {
        active = false;
    }

    private void ApplyStall()
    {
        List<GameObject> otherObjects = DetectObjectsWithinRadius(transform, range);

        if (otherObjects.Count > 0)
        {
            foreach (GameObject otherObject in otherObjects)
            {
                BaseCharacter canStall = otherObject.GetComponent<BaseCharacter>();

                if (canStall != null)
                {
                    Debug.Log("Tried to stall: " + otherObject.name);
                    canStall.stall.ApplyStall();
                }
            }
        }
    }
}
