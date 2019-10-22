using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class StallTrap : Trap
{
    Animator anim;
    SpriteRenderer sRenderer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Activate()
    {
        if (!active)
        {
            InitializeTimer();
            anim.SetTrigger("activate");
            ApplyStall();
        }   
    }

    public override void Deactivate()
    {
        active = false;
        anim.SetTrigger("deactivate");
        RemoveStall();

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
                    canStall.GetComponent<Rigidbody2D>().MovePosition(transform.position);
                    sRenderer.sortingOrder = 1;
                }
            }
        }
    }

    private void RemoveStall()
    {
        sRenderer.sortingOrder = -1;

        List<GameObject> otherObjects = DetectObjectsWithinRadius(transform, range);

        if (otherObjects.Count > 0)
        {
            foreach (GameObject otherObject in otherObjects)
            {
                BaseCharacter stalled = otherObject.GetComponent<BaseCharacter>();

                if (stalled != null)
                {
                    //Debug.Log("Release: " + otherObject.name);
                    stalled.stall.RemoveStall();
                }
            }
        }
    }
}
