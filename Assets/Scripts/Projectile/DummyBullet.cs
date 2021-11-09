using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Dummy Bullet. Do nothing. 
public class DummyBullet : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        recycleProjectile(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
