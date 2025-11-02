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

        private void Start()
        {
            playerHealth = GetComponentInParent<Player_Health>();
            interactionCircle.enabled = false; 
            failedInteractionCircle.enabled = false;
        }

        public void UpdateHealthBar()
        {
            healthBarImage.fillAmount = playerHealth.health / 100;
        }

        public void UpdateLootScoreText()
        {
            // lootScoreText.text = ?
        }

        public void EnableInteractionCircle()
        {
            StartCoroutine(InteractionCircle());
        }

        public void EnableFailedInteractionCircle() //Called on failedInteraction event (At Door Logic)
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
