using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Powerup Effect")]
public class GenericBuff : PowerupEffect
{
    public BuffCategoryEnum BuffCategory;
    public float Value;

    public override void Apply(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        switch (BuffCategory)
        {
            case BuffCategoryEnum.MAX_AMMO:
                player.MaxAmmo += (int) Value;
                player.FinishReload();
                break;
            case BuffCategoryEnum.BULLET_DAMAGE:
                player.BulletDamage += Value;
                break;
            case BuffCategoryEnum.FIRE_RATE:
                player.FireRate += Value;
                break;
            case BuffCategoryEnum.RELOAD_RATE:
                player.ReloadRate += Value;
                break;
            case BuffCategoryEnum.BULLET_SPEED:
                player.BulletSpeed += Value;
                break;
            case BuffCategoryEnum.MOVEMENT_SPEED:
                player.CurrentMovementSpeed += Value;
                break;
            case BuffCategoryEnum.DASH_COOLDOWN:
                player.UpdateDashCooldown(Value);
                break;
            case BuffCategoryEnum.BULLET_TIME_COOLDOWN:
                player.UpdateBulletTimeCooldown(Value);
                break;
            case BuffCategoryEnum.HEAL:
                player.UpdateHealth(Value);
                break;
            case BuffCategoryEnum.MAX_HEALTH:
                player.UpdateHealth(0, Value);
                break;
        }
    }
}

public enum BuffCategoryEnum 
{
    MAX_AMMO,
    BULLET_DAMAGE,
    FIRE_RATE,
    RELOAD_RATE,
    BULLET_SPEED,
    MOVEMENT_SPEED,
    DASH_COOLDOWN,
    BULLET_TIME_COOLDOWN,
    MAX_HEALTH,
    HEAL
}
