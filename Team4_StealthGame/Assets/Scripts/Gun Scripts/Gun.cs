using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// gun base class
public class Gun : Item
{
    // references
    [SerializeField] protected Transform gunBarrelEnd;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Animator anim;

    // stats
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float timeBetweenShots = 0.1f;
    [SerializeField] protected bool isAutomatic = false;

    // private variables
    protected int ammo;
    protected float elapsed = 0;
    protected bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
    }

    public int GetAmmo() => ammo;
    public int GetMaxAmmo() => maxAmmo;

    public override void Equip(Player_Inventory p)
    {
        base.Equip(p);
        p.GetComponent<Player_GunHandler>().enabled = true;
    }

    public override void Unequip() { player.GetComponent<Player_GunHandler>().enabled = false; }

    public bool AttemptAutomaticFire()
    {
        if (!isAutomatic)
            return false;

        return true;
    }
    [Rpc(SendTo.Server)]
    public virtual void AttemptFireRpc()
    {
        if (ammo <= 0)
        {
            canShoot = false;
        }

        if(elapsed < timeBetweenShots)
        {
            canShoot = false;
        }

        canShoot = true;
    }

    public virtual void AddAmmo(int amount)
    {
        ammo += amount;

        if (ammo > maxAmmo)
            ammo = maxAmmo;
    }
}
