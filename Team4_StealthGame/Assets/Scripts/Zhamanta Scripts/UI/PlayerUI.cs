using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Cali;


namespace Zhamanta
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] Player_Health playerHealth;
        [SerializeField] PlayerLogic playerLogic;
        [SerializeField] TMP_Text lootScoreText;
        [SerializeField] Image healthBarImage;

        [SerializeField] Image interactionCircle;
        [SerializeField] Image failedInteractionCircle;

        private float elapsedInteractionTime;

        //Lock Visuals
        [SerializeField] Image lockedImage;
        [SerializeField] Image unlockedGif;

        private void Start()
        {
            playerHealth = GetComponentInParent<Player_Health>();
            playerLogic = GetComponentInParent<PlayerLogic>();
            interactionCircle.enabled = false; 
            failedInteractionCircle.enabled = false;
            lockedImage.enabled = false;
        }

        public void UpdateHealthBar() //CALL THIS when player gets hurt
        {
            healthBarImage.fillAmount = playerHealth.health / 100;
        }

        public void UpdateLootScoreText() //CALL THIS when collecting loot.  Feel free to edit this script and finish this function.
        {
            // lootScoreText.text = ?
        }

        // Lock visuals
        public void ShowLockedImage() //CALL THIS when trying to open locked door
        {
            StartCoroutine(LockedImageTimer());
        }

        IEnumerator LockedImageTimer()
        {
            lockedImage.enabled = true;
            yield return new WaitForSeconds(1);
            lockedImage.enabled = false;
        }

        public void ShowUnlockedGif() //CALL THIS when a door has been successfully unlocked
        {
            StartCoroutine(UnlockedGifTimer());
        }

        IEnumerator UnlockedGifTimer()
        {
            unlockedGif.enabled = true;
            yield return new WaitForSeconds(2);
            unlockedGif.enabled = false;
        }


        // Interaction Circles
        public void EnableInteractionCircle() //CALL THIS when the player is interacting
        {
            StartCoroutine(InteractionCircle());
        }

        public void EnableFailedInteractionCircle() //CALL THIS on failedInteraction event found in the DoorLogic script
        {
            StartCoroutine(FailedInteractionCircle());
        }

        IEnumerator InteractionCircle()
        {
            interactionCircle.enabled = true;
            elapsedInteractionTime = 0;
            while (elapsedInteractionTime < playerLogic.interactTime)
            {
                elapsedInteractionTime += Time.deltaTime;
                interactionCircle.fillAmount = elapsedInteractionTime / playerLogic.interactTime;
                yield return null;
            }
            if (elapsedInteractionTime == playerLogic.interactTime)
            {
                if (interactionCircle.enabled)
                {
                    interactionCircle.enabled = false;
                }
            }
           
            yield return null;
        }

        IEnumerator FailedInteractionCircle() 
        {
            interactionCircle.enabled = false;
            failedInteractionCircle.enabled = true;

            float interactionTimeAchieved = elapsedInteractionTime;
            failedInteractionCircle.fillAmount = interactionTimeAchieved / playerLogic.interactTime;
            yield return new WaitForSeconds(.5f);

            failedInteractionCircle.enabled = false;
            yield return null;
        }
    }
}
