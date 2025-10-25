using UnityEngine;

namespace Notetaking
{
public class ToDo10_25
{

}
}

/*
 * 	
							Lil Disclaimer (I Am Autistic and Anxious Preamble)
	I do not want this to be pointed or come across as rude, I am simply wanting to express my wants for this project.
	If I am wording my wishes as more of complaints or just in a way that is damaging to someone's feelings please-
	-let me know so I can address my language and manner when expressing plans/ideas. I have done critiques and evals-
	-of all kinds so I am comfortable being blunt to others about their work and receiving blunt feedback!
====================================================================================================================================================

----------------------------------------------------------PRIORITY GOALS----------------------------------------------------------------------------
In General:
	Set up Player, Guard, and UI Scripts to be more easily accessible by other scripts and to be expanded upon. On top of general functionality.
		I don't mind being the main person to figure out how to merge/organize the scripts when we want to combine our work properly.
	Even if I do not do the merging troubleshooting alone, commenting and explanations in-script would be very beneficial and good practice overall.
	Specifications on preferences/ideas are all below, if there are any questions I should still be able to respond on Discord if I'm awake lol.
	Again, I want to reiterate that I want y'all to put your own spin/influence on this project so feel free to implement/mess around with w/ever!

Carlos:
	For Player specifically, I have listed a lot more detailed version of what I am wanting/thinking about for the Player scripts.
	The tl;dr is this: Clean them up, remove the extraneous stuff, split up the scripts, and set up some events/public methods for others to use.
	Don't worry about the NetworkManager stuff and if you want to remake your scripts all the more power to you, just make sure to organize them.
	Also with importing stuff, I mention it below but to help with keeping things clean please just put the bulk of stuff in a folder when u import.
	Then you can take what you need from that import and set it up for our project specifically, it's not a big problem but my OCD *is* screaming.
	And to reiterate: you have free reign with this project as much as I do. Just bc it's my idea doesn't mean you can't go crazy on it if u want.
	Don't overwork yourself or stress out too much over the small stuff, I would just really appreciate modifying the scripts to fit this project.
	Organization is also a really good habit to get into with coding, but as we've discussed in class, it's not the end-all be-all of gamedev.
		Appreciate you gamer! Have fun and let me know if you end up reading through any of my scripts and have any comments.

Zhamanta:
	As mentioned above, there are more specific ideas/concepts/requests below but I haven't really put much thought into specific UI implementation.
	You're free to do whatever you want with the UI, and if you need us to make any assets just let us know! Idm doing the models for level stuff.
		I mainly just want some specifications on how to call/interact with your scripts and events, and the intention behind them.
	Preferably in-script so we don't have to navigate to other windows or require internet connection/Discord being up, but I'm sure you know that.
	You're both capable programmers, and you are already implementing good organizational practices and conventions! So no complaints from me.
		If you prefer to leave the decision of what the effects of the different alarm stages are to me, that's cool just lmk.
	As I outline below I just want some more outlining/structuring and explicit direction with your intentions for UI/animation elements.
			I really appreciate the work you do and tolerating my capacity for yapping, been a pleasure working with you!

----------------------------------------------------------------------------------------------------------------------------------------------------

====================================================================================================================================================	
General Wishes for Scripting/Organization:
	Focus on using UnityEvents or public methods for triggering state changes/reactions to player interactions.
		- This also goes for logic handled within a single script: if it notably impacts gameplay then it should be accessible.
			} Now this doesn't go for things ran in Update necessarily, or internal vars like storing previous states.
			} However, things that track/activate these changes SHOULD be accessed in-script by public methods/public events.
		- This isn't something that is black/white either, just if it feels like something that might be needed to be accessed...
			...or noted by other objects, make sure they're able to do that.
			} But also we don't need to flood the inspector and intellisense with 50000 things to track/call yk.
		- I generally don't rely on [SerializeField] and prefer to use relative calls/assignments in case of user error.
			} When I say user error I also VERY MUCH include myself in that, I can be real ditzy and brain-off sometimes.
			} For component references that are on the same object especially, or GameObjects there are only one of I don't use SF.
			} However for stuff for testing/changing in run-time (speed/mass/reaction time etc.) HIGHLY want those in SF.
				- There might be a way to set it up so they only appear when we activate a "Testing Mode" but idk how to do that exactly.
		- To emphasize the point of this with an example, take lockpicking a door:
			} Player has to enter the collision/interaction range of the door.
			} Door has to recognize that the player is actively within range and await them attempting to interact with the door.
				- Already this is most easily done with an event so we don't have to run checks every second on both ends.
			} After beginning to lockpick multiple things need to happen across multiple scripts.
				- Player might have an animation that plays when lockpicking that might need to be interrupted.
				- The door could also have an animation or some interaction when the player fails/stops early.
				- There probably is some UI element that will update to show the progress of the player and give feedback.
				- If not already suspicious, lockpicking definitely should make them suspicious so that needs to be updated for guards.
				- Managers for guards and doors and sound also probably have some level of interaction depending on what happens to the door.
			} So to communicate across all these scripts simultaneously and have them react in time, events make the most sense.
			
	Clear naming even if redundant. If there is a local variable that's less of an issue but please don't just make it "o" or something.
		- I dont mind doing the script merging, but this is why I ask for clear naming so I can make sure we don't have literal redundancies.
		- We're doing a bank heist game, if not "Player" then "Robber" and not "Enemy" but "Guard" because they might not be always hostile.
			} This is v much an OCD thing for me but I just, I prefer clarity in my naming conventions no matter the scope/level.
	Scripts should be in folders/namespaces and should detail what they are attached to.
		- EX: "PlayerMovement" over "Movement" because we have multiple scripts for handling the movement of various entities.
	Plug and play from previous assignments is ok and encouraged since we know it works! Just update the wording and remove extra fluff.
		- Scripts for v specific things shouldn't handle logic not associated with the namesake.
			} EX: PlayerMovement handles the movement, but we have a separate script for aiming/turning.
			} This isn't just for cleanliness but also for debugging and efficiency since only crucial/needed info will be pooled...
			...and referenced in the same manner/from the same place.
		- Anything imported from other projects *please* put into their own folder so we reduce clutter.
====================================================================================================================================================		
Specific Goals for Scripts!

(Individual) Guard Scripts:
	Uniformity in naming; i.e., make them all "Guard__x__" with x being the specific aspect that script is handling.
		- Some ideas for x: Movement, StateMachine, ReactionHandler, EventHandler
			} Mine rn are: GuardBase, GuardStats (which are both used as templates for derived scripts) and GuardLogic (placeholder for interaction testing)
		- With things like EventHandler/ReactionHandler we can centralize access to objects to, again, make debugging/testing clearer.
			} Albeit, this does make it a *bit* more complicated to implement from the entry, but once we get the base set up I think it'll be good!
			} Like if we're having: an event not adding properly, NullRefException, value calculating improperly we can diagnose it easier.
				- There may be more scripts it goes through, but we can gradually thin down which scripts are even interacting with said process.
				- If EventHandler -> Logic -> (input StateChange) StateMachine -> Movement -> EventHandler then we can...
				...remove access/calls one by one until the error disappears which helps cut down on checking methods line-by-line.
	Specific public method/events wanted:
		- InterruptPatrol: something to alert guards and send them to a location.
			} Not sure if we'd wanna differentiate between seeing like, a broken door/player and something auditory, up to y'all
				- Could just have different modes of reaction, which would work well with the general alertness level mechanic.
			} I implemented a reaction time in one of my mock-up methods to allow players to mess up an interaction and...
			...not immediately have guards chasing them. It feels more natural as well to have guards slowly turn to minor sounds.
		- Combat (maybe with Begin/End): to toggle whether or not the guards are in "combat mode"
			} I planned on, if the GuardManager/security center is still active, all guards get a consistent update on players'...
			...positions as long as one guard/camera can see them or is engaged with them.
			} I want a way to send them back to an alerted but not tracking players state in case players manage to escape.
				- Entails: shorter reaction times, faster recognition of players, increased guard presence, exits available changed...
				...all areas become restricted, etc.
				- This isn't priority, but while implementing guards being able to combat players I wanted it to be a consideration.
		- Methods that override for different kinds of guards
			} Basically decide if we want to have a variety of guards for the Director to spawn such as:
				- PoliceCaptain, Shieldbearer/FrontLine, Medic, Sapper/ElectronicsSpecialist*, Stealth/Cloaker
					} Basically just using Payday 2's police variety as an example, but I think it's a good reference for variety.
					}* Very extra idea, but also in Payday police sometimes try to turn out the power on you and have NVG and flashlights.
			} Other than different police when on alert, I would also like at least one alternative that acts like a surveillance guy.
				- Like Payday 2, if you take them out then cameras won't spot you and we can have it reduce alertness level.
				
Player Scripts:
	Uniformity in naming (just like with Guards; i.e., "Player__x__")
		- Some script specialization ideas: Movement, Aiming, NoiseHandler, InteractionHandler, StatusUpdater, Weapons/Inventory, Attack
		- If we do an EventHandler script for the guards, all the more reason we should do it for the player scripts.
	Specific public method/event requests:
		- Health: Since we discussed having healing items, wouldn't hurt to have a script handle their effects.
		- NetworkUpdater: Handle passing values from other scripts to the NetworkManager, not priority but will be nice to have later.
		- UIUpdater: Handle and centralize info passing through to the UI. Not necessary but could be nice.
		- TeamInteractions: Handle interacting with other players, like picking them up, healing them, deciding if to collide with them.
		- Appearance/Suspicion: Communicate to other scripts about whether guards will suspect them and if they draw attention.
		- Stealth: Similar idea, but specifically for sneaking and maybe hiding in closets/bins if we thats something we want.
		
UI Scripts
	Pretty happy with how Zhamanta is structuring her stuff so far, no changes necessarily needed.
		- Just need some commenting for intentions behind different variables/methods.
		- Such as what you were planning for the different alarm stages and what the directorAI would need to call.
	Specific public method/event requests:
		- Hard to say exactly what I am wanting here since Z is on UI, but in general what I will require to integrate/interface with... 
		...your work is a list of UI elements you're planning to make and how to call/update them.
		- I will provide a list of the interactions I can think of so far, delineating ones I've implemented from conceptual ones.
			- Implemented; i.e., ones I have already accounted for in my scripts:
				} DoorInteraction: I just thought of a simple circle radially filling up, and have made space in coroutines for that.
				} GuardAlert: Already made an event to alert guards of when a noise is made, and it is modifiable by parameter.
				} ToggleLockdown: Event set up for triggering lockdown and sealing all doors, which each handle the change individually.
					- Ideally there is something we could call in the same method invoked by that event to visually update them, and I...
						...can make the various models just lmk if you have any ideas/requests.
				} PlayerLastKnown: Not an event but a variable I've accounted for ahead of time
					- Thought of having a ghost to communicate to the players the last location they've been noticed/seen while guards are otw.
				} OnDestroy: My GuardLogic has an example of an OnDestroy call for GuardManager.
					- Can use the same inherited method or set up an event to be invoked for any object being destroyed.
					- Could use it also to update UI when Player collects a Valuable and it's destroyed
					
			- Concepts; just ideas, no groundwork laid in my scripts
				} ItemAnimation: A method to feed a prefab/objref/position for pick-up-able objects around the world.
					- Could give them a slight backglow or have them slowly bob up and down.
				} AddPointer: Method that can be fed object references/positions to update UI to add a pointer for players/a player.
					- In case we want to add more interactivity/feedback for players so they can know whats going on off-screen.
					- Guard finding broken door, a dead body, a weapon, etc.
				} LockdownVisuals: Method that handles general visual changes when lockdown is active.
					- Player weapons being constantly visible.
					- Whatever UI elements would alert the player that lockdown is going off.
					- Whenever a wave of guards is spawned a pointer for like, 10 seconds to where they are spawning.
				} PlayerInteractFeedback: Method that takes a player reference and a gameobject reference
					- A more general method for handling how the UI should respond to players attempting to interact with objects.
						} I have a handler for the doors that processes successful player interactions that is just an enum switch case.
						} Could have the method take the two object references or two script references so it knows what object is being...
						...interacted with from the start.
						} Could also make overrides where its still just 2 gameobjrefs and a third parameter of an object name/enum/int w/ever.
	Get a list together of UI elements you plan on making and what you would need us to do.
		- As said I can make the level component models, but for the actual UI sprites and player feedback UI I understand that is your job.
		- Either way just compile a list of what you have and will make, what you might need help with, and anything you can think of we need to make.
 */
