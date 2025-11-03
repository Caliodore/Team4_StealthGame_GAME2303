using UnityEngine;

public class Zhamanta_Update_11_2 : MonoBehaviour
{
}


/*


- NOTES WHEN YOU READ THROUGH MY CODE

I commented "CALL THIS" in all caps next to the functions that are meant to be implemented by you guys!
Any DirectorAI functions that have such comment are to be called exclusively through events.

You need to read my DirectorAI, GlobalUI, PlayerUI, and MediumUI scripts.


- DIRECTOR AI SUMMARY

The DirectorAI is basically finalized!

I already made a gameObject in the hierarchy called DirectorAI that holds the script, and I assigned the functions...
...to the OnAlertnessStageChange event.

Not sure if hierarchy changes will merge easily to main branch.


- UI SUMMARY

I have made three different UI scripts: GlobalUI, PlayerUI, and MediumUI.

GlobalUI holds the functions of visual events that need to be seen by every player, such as the alarm going off...
...or the lights turning off.

PlayerUI will handle visual events unique to each player.  Ex: when a player is interacting with a door, only they...
...should see the lockpickImage appear in their screen.  I assigned the canvas with the PlayerUI as a child of the...
...NetworkPlayerTest prefab, so that each player spawned will have their own canvas with their own PlayerUI.
...Ex: the health bar simply fetches the health info from the parent gameobject (aka NetworkPlayerTest).

There are functions in the PlayerUI that need information from scripts that are not contained in the...
...NetworkPlayerTest.  This is where the MediumUI comes in.  Ex: if a door gets unlocked, it needs to call...
...the function from the MediumUI that is in charge of activating the unlockedImage, and it needs to pass the...
...PlayerLogic of the player that unlocked the door.  This will make it so that only that player sees the...
...unlockedImage.


- EXTRA NOTES

PlayerLogic is currently not a component of the NetworkPlayerTest prefab.  This needs to be corrected.

Please do not miss my comment next to the UpdateLootScoreText() in the PlayerUI script.
Please do not miss my comments under the IEnumerator InteractionCircle()  in the PlayerUI script.

The only visual I am missing in the GlobalUI is the targetImage (the valuable).  Please assign it in the...
...GlobalUI gameobject if you have it/are done creating it.

Every script of mine is under the namespace Zhamanta.


- FINAL NOTES/COMMENTS

This is the last weekend I can work on this.  I will probably not be able to work on this project anymore, but...
...I will answer any questions you guys may have related to my work.  Feel free to make changes to my scripts, but...
...please let me know what you are changing.

Example: feel free to finish the UpdateLootScoreText() in the PlayerUI script, but send me a quick message about it.

If you really need me to work on something, let me know and I will make time!

It has been AMAZING to work with you guys!


 */
