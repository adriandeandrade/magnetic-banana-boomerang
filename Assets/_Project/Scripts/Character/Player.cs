﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class Player : BaseCharacterMovement
{
    public override void Update()
    {
        HandleMovement();
    }


    private void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        // Clamp the magnitude so we cant move faster diagonally.
        Vector2 vel = new Vector2(horizontal, vertical);
        vel = Vector2.ClampMagnitude(vel, 1);

        base.Update();

        Move(vel);

    }
}
