using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        this.lifetime = 0f;
        base.OnTriggerEnter2D(collider);
        var player = collider.GetComponent<Player>();
        var uiPlayer = UIPlayer.Instance;
        uiPlayer.SetHealth(player.GetCurrentHealth(), player.GetBaseHealth());
    }
}
