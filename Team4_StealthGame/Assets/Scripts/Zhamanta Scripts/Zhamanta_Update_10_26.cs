using UnityEngine;

public class Zhamanta_Update_10_26 : MonoBehaviour
{
}


/*

- SUMMARY
This weekend, I tried to implement nearly all methods/variables that are meant to be used by the Director AI found in the managers.

Please read my comments in the DirectorAI, and watch out for definitions of Problems A-G.
Example:
Next to a method or initialization or anything, I might have commented "Problem A: I need x from the y manager in order to do z."
Then I proceed to comment "Problem A" in areas affected by that problem.
Not everything you need to read has "Problem" next to it, so read carefully as there are some parts where I simply ask for help or opinion.

Carlos, I think it is better if you make the host/join canvas panel directly on the 2ndBankAttempt scene of the main branch because I believe...
...that if I make changes in the 2ndBankAttempt of my branch, it will cause merge conflicts.  Merging scripts is easy, but messing with scenes...
...and objects in the hierarchy tends to create problems I believe.

- CONFUSION
I am confused about the goal in our game.
What I thought the goal was: to collect one valuable item (I have been calling it a jewel, could be the president's case with millions of cash)...
...Additionally, the player can collect money here and there. Escape with the big valuable without being killed or arrested.

However, it does not seem to me like that is what we are doing anymore.

I just see "valuables" and I am not sure what those are.  
When you collect "valuables" are those money?  "You got $10!" or "You got one valuable!"? 
Can the player escape without collecting the one big valuable (the jewel or whatever)?  Do we even have that one big valuable anymore?
Are "valuables" items rather than cash/coins?

Things to think about:
If a player escapes, and others are still alive, do they get an individual "You escaped" screen? And when they all escape, they get one united...
..."You win" screen?
Similarly, if one dies or gets arrested, do they get an individual "You died/got arrested" screen? And if they all die, they get a united...
..."You all died" screen?  If everybody died but you, do you get a "Your friends would be proud" screen? And those who are dead get a...
..."Your friend did it for you" screen?
I have to ask this because I am planning to make any end-of-game screens or panels we may need

- UI I WILL MAKE
> Health bar
> Interaction Circle
> Any Text
> Handle any light/color changes in the screen (flashing lights/turning off lights)
> Closed Lock Image that will pop if the player attempts to open a locked door
> Lockpick Image/Animation enabled during the time the player is lockpicking a door
> Opening Lock Image/Animation that will pop if the player successfully lockpicks a door
> Fist Image/Animation that will pop anytime the player successfully hits an enemy
> K.O Image/Animation that will pop anytime the player knocks out a guard
> Skull Image/Animation that will pop anytime the player kills a guard
> Win/Lose/Whatever screens/panels (please clarify which ones we are doing)

-UI / MODELS I NEED OTHERS TO MAKE
> Whatever the "valuables" are.  If it is just money or cash, I need something representative of it to scatter across the map...
...If they are actual items, like a jewel, an expensive watch, etc., I need those images.  In that case, I could make a storage system...
...that will show the items you have collected.
> Locked Door model
> Unlocked Door model 
> Gun
> Bullets
> Player
> Guards
> Any dying animations if any
> Player/guard flashlights and their behavior if there are any

Most of these images/animations will be enabled/disabled through events. 

- ANSWERING YOUR QUESTIONS / ASKING QUESTIONS ABOUT YOUR TO_DO_10_25
Yes! differentiating the reaction between seeing a player/broken door vs hearing something is a good idea!
I think the main concern is to ensure the guards enter/exit combat mode at appropriate times.  Perhaps when a player shoots a guard, they all...
...enter combat mode, and they exit combat mode when the player is badly hurt.  At this point, they could just enter arrest mode...
...and chase you until they catch you.
We only have a week and a half left (I think) so... if you want to have a variety of guards, you are in charge of it, hehe.

 */


