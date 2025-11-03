using Cali;
using UnityEngine;

namespace Zhamanta
{
    public class MediumUI : MonoBehaviour
    {
        public void ShowSkullImage(PlayerLogic interactingPlayer) //CALL THIS when a guard is killed (probably called by a guard)
        {
            PlayerUI interactingPlayerUI = interactingPlayer.gameObject.GetComponent<PlayerUI>();
            interactingPlayerUI.ShowSkullImage();
        }

        public void ShowKOImage(PlayerLogic interactingPlayer) //CALL THIS when a guard is knocked on (probably called by a guard)
        {
            PlayerUI interactingPlayerUI = interactingPlayer.gameObject.GetComponent<PlayerUI>();
            interactingPlayerUI.ShowKOImage();
        }

        public void ShowLockedImage(PlayerLogic interactingPlayer) //CALL THIS when trying to open locked door (probably called by a door)
        {
            PlayerUI interactingPlayerUI = interactingPlayer.gameObject.GetComponent<PlayerUI>();
            interactingPlayerUI.ShowLockedImage();
        }

        public void ShowUnlockedGif(PlayerLogic interactingPlayer) //CALL THIS when a door has been successfully unlocked (probably called by a door)
        {
            PlayerUI interactingPlayerUI = interactingPlayer.gameObject.GetComponent<PlayerUI>();
            interactingPlayerUI.ShowUnlockedGif();
        }

        public void EnableFailedInteractionCircle(PlayerLogic interactingPlayer) //CALL THIS on playerFailsInteraction event found in the DoorLogic script (it needs a playerLogic parameter)
        {
            PlayerUI interactingPlayerUI = interactingPlayer.gameObject.GetComponent<PlayerUI>();
            interactingPlayerUI.EnableFailedInteractionCircle();
        }
    }
}

