using UnityEngine;

public class Notes_NOT_CODE
{    
}

/* Explanation of Cali's Scripting Methodology
 * General remarks:
 *      - My code can be overly complicated for the sake of future-proofing or being precise and clear with wording/naming.
 *      - In the same vein my code tries to self-contain a lot of references that are needed and specifically does NOT use the Inspector except for debugging.
 *      - Also, most of my methods aren't intended to be accessed and changed without directly asking as to how I intend them to be expanded.
 *      - Methods that have empty space/placeholder values or seemingly empty sections will (hopefully) have comments and notes detailing why that space might need to be accessed later.
 *          } Prime example: In the player interaction with the doors I have some coroutines to update UI and server feedback that I haven't but anything in except a note about UI.
 *      - HOWEVER, I want to make sure that my code *is* readable and usable by others and if anything I do/make is unclear I would love to know!
 *          } Feel free to put some comments within my scripts if things are unclear and I can't answer it right then or if you feel an area needs more commenting!
 *      - I do a lot of planning and emphasis on structuring stuff before building actual physical game components because again: I want to make sure the code makes sense reading through it/on its own.
 *      - A lot of the things I implement/set up that seem extraneous probably are! I'm normally trying to understand the tools as best as I can and use them however comes to mind.
 *          } I am very ADHD and OCD though so there are many times I make structures and they are not as useful as I think they will be (see my dictionaries ~n~).
 *              - My workflow is also HORRENDOUS and I get distracted/forget about things I'm working on a lot because I get a eureka and have to test it >W<.
 *          } But! when experimenting like this I find it helps a lot to talk to others about it because the hardest part of any field is how to communicate your ideas properly.
 *              - In that same vein, knowing HOW to ask your questions makes a HUUUUUUUGE difference and I struggle with that a fair amount because I get confused by basic terms.
 *      - I also have generally expected myself to go towards solo game-dev instead of a studio/team but found that a lot of these tools for doing group work help a lot with personal organization!
 *          } I say this mainly because it's ok (and probably better in some cases) to want to focus on certain aspects of coding/dev rather than try to be as wide as possible.
 *          } Jack of all trades, master of none; I may be able to do a lot of different aspects but a team with specialists can do a lot more work in MUCH less time even accounting for manpower.
 *      - I hope none of this has come off as pretentious or mean-spirited, I just wanted to try and deconstruct the way I tackle coding as a whole and hope that all of this helps!
 *          } TL;DR: I try to do a lot of stuff outside of my reach cuz it helps me learn better personally. It can be messy and convoluted but it's just how I know!
 *          }        Coding is v much the process of solving puzzles to get more puzzles to solve; fun and frustrating in the same breath. :3
 */

 //* ------------------------------------------------------------------------------------------------------------------------------------------------------
 //* 
 //* ====================================================================================================================================================
 //* More yapping about how I approach the various bits of development and coding
 //* ------------------------------------------------------------------------------------------------------------------------------------------------------
 
/* Manager Scripts
 * |---------------------------------------------------------------------------|
 * General Intent:
 * Managers are an in-between for GameObjects and pure logic objects to go through so that it is easier to keep tabs on how things are processed.
 * They can be somewhat of a catch-all for what they handle: logic, data storage, UI manip, w/ever is needed.
 * What's important is that they have a jurisdiction
 * 
 * 
 */
