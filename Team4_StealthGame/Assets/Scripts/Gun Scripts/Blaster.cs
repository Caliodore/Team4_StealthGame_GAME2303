using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Blaster : Gun
{

    [Rpc(SendTo.Server)]
    public override void AttemptFireRpc()
    {
        if (ammo <= 0)
        {
            canShoot = false;
        }

        if (elapsed < timeBetweenShots)
        {
            canShoot = false;
        }

        GameObject b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(3, 100, 2, 0.0f, null); // version without special effect
        b.GetComponent<NetworkObject>().Spawn();
        // test

        //anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        canShoot = true;
    }

    // example function, make hit enemy fly upward
    void DoThing(HitData data)
    {
        Vector3 impactLocation = data.location;

        var colliders = Physics.OverlapSphere(impactLocation, 1);
        foreach(var c in colliders)
        {
            if(c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
        }
    }
}
