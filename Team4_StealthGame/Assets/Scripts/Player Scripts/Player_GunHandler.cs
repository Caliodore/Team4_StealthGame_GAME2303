using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/****************** Summary of Player Gun Handler ******************/

// This script holds all the player logic handling the Gun
// via player doing actions: reloading, shooting, and more (if we make the necessary add ons/changes)

/*******************************************************************/

public class Player_GunHandler : MonoBehaviour

{
    // private gun variables
    Gun currentGun = null; 
    bool isShooting = false;

    void OnShoot(InputValue v)
    {

        if (currentGun == null)
        {
            print("Gun is null returned");
            return;
        }

        isShooting = v.isPressed;

        Debug.Log("OnShoot Message Called!");

        if (isShooting)
        {

            currentGun?.AttemptFire();

        }


    }

    void OnReload()
    {
        if (currentGun != null)
        {
            currentGun.AddAmmo(999); // cheat code for ammo
        }
    }


    /// <summary>
    /// This function should be called in the inventory script <br/>
    /// The reason is the way the Gun Script was originally coded.
    /// Using many return bool functions to attempt fire and checking if Gun itself is null. <br/> 
    /// The inventory doesn't check this so we need to call this function if the current object is type Gun as well.
    /// </summary>
    public void SetGun(Gun gun)
    {
        currentGun = gun;   
    }







    // Carlos:
    // if needed later if we make an automatic gun (we probably will)
    // just wanted to move this out the way :D

    /*void AutomaticFire()
    {
    if (currentGun == null)
        return;

    if (isShooting)
    {

    if (currentGun.AttemptAutomaticFire())
    {
        Debug.Log("AUTOMATIC GUN FIRING");
        currentGun?.AttemptFire();
    }

    }
    }*/

}
