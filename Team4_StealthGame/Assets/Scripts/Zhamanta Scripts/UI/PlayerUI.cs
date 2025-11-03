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

        //Basic UI visuals
        [SerializeField] TMP_Text lootScoreText;
        [SerializeField] Image healthBarImage;

        //Interaction Visuals
        [SerializeField] Image interactionCircle;
        [SerializeField] Image failedInteractionCircle;

        private float elapsedInteractionTime;

        //Lock Visuals
        [SerializeField] Image lockedImage;
        [SerializeField] Image unlockedImage;

        //Other visuals
        [SerializeField] Image lockpickingImage;
        [SerializeField] Image KOimage;
        [SerializeField] Image skullImage;

        private void Start()
        {
            playerHealth = GetComponentInParent<Player_Health>();
            playerLogic = GetComponentInParent<PlayerLogic>();

            interactionCircle.enabled = false; 
            failedInteractionCircle.enabled = false;

            lockedImage.enabled = false;
            skullImage.enabled = false;
            unlockedImage.enabled = false;
            lockpickingImage.enabled = false;
            KOimage.enabled = false;
        }

        public void UpdateHealthBar() //CALL THIS when player gets hurt
        {
            healthBarImage.fillAmount = playerHealth.health / 100;
        }



        public void UpdateLootScoreText() //CALL THIS when collecting loot.  I can't find where you are keeping track of the loot score.  If the player is not the one calling this, create a function for it in MediumUI.
        {
            // lootScoreText.text = ?
        }



        public void ShowSkullImage() //Go to MediumUI
        {
            StartCoroutine(SkullImageTimer());
        }

        IEnumerator SkullImageTimer()
        {
            skullImage.enabled = true;
            yield return new WaitForSeconds(1);
            skullImage.enabled = false;
        }



        public void ShowKOImage() //Go to MediumUI
        {
            StartCoroutine(KOImageTimer());
        }

        IEnumerator KOImageTimer()
        {
            KOimage.enabled = true;
            yield return new WaitForSeconds(1);
            KOimage.enabled = false;
        }



        // Lock visuals
        public void ShowLockedImage() //Go to MediumUI
        {
            StartCoroutine(LockedImageTimer());
        }

        IEnumerator LockedImageTimer()
        {
            lockedImage.enabled = true;
            yield return new WaitForSeconds(1);
            lockedImage.enabled = false;
        }

        public void ShowUnlockedGif() //Go to MediumUI 
        {
            StartCoroutine(UnlockedGifTimer());
        }

        IEnumerator UnlockedGifTimer()
        {
            unlockedImage.enabled = true;
            yield return new WaitForSeconds(2);
            unlockedImage.enabled = false;
        }



        // Interaction Circles
        public void EnableInteractionCircle() //CALL THIS when the player is interacting
        {
            StartCoroutine(InteractionCircle());
        }

        public void EnableFailedInteractionCircle()  //Go to MediumUI
        {
            StartCoroutine(FailedInteractionCircle());
        }

        IEnumerator InteractionCircle()
        {
            //lockpickingGif.enabled = true;  This assumes interacting with a door means to lockpick it.  If interaction is not exclusive to lockpicking, then this needs to be revised.
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
                    //lockpickingGif.enabled = false;
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
