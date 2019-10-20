using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public interface IDamageable
{
    void TakeDamage(float amount, Vector2 damageDirection);
    void TakeDamage(float amount, Vector2 damageDirection, BaseCharacter damageSender);
}
