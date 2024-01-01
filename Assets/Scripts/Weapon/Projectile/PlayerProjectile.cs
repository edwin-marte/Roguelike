using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public GameObject spawnTextPrefab;
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        base.OnTriggerEnter2D(collider);

        //Spawn Damage Text
        var spawnText = Instantiate(spawnTextPrefab, transform.position, Quaternion.identity);
        spawnText.GetComponent<SpawnTextMesh>().SpawnText(GetDamage().ToString(), transform.position, spawnText);
    }
}
