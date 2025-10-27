using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Cali;


namespace Testing
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] PlayerStats playerStats;
        [SerializeField] PlayerLogic playerLogic;
        [SerializeField] TMP_Text moneyText;
        [SerializeField] TMP_Text alertnessText;
        [SerializeField] Image healthBarImage;
        //[SerializeField] Image jewelImage;

        [SerializeField] Image interactionCircle;
        [SerializeField] Image failedInteractionCircle;

        private float elapsedInteractionTime;

        private void Start()
        {
            playerLogic = FindFirstObjectByType<PlayerLogic>();
            interactionCircle.enabled = false;
            failedInteractionCircle.enabled = false;
            //jewelImage.enabled = false;
        }

        // Could use UnityActions/Events instead of Update() for optimization
        private void Update()
        {
            UpdateHealth();
            UpdateAlertness();
            UpdateMoneyCount();
        }
        public void UpdateHealth()
        {
            //healthBarImage.fillAmount = playerStats.currentHealth / playerStats.maxhealth;
            healthBarImage.fillAmount = playerStats.health / 100;
        }

        public void UpdateAlertness()
        {
            alertnessText.text = "Alertness: " + AlertnessLevel.alertnessL.ToString();
        }

        public void UpdateMoneyCount() //OnMoneyCollected event?
        {
            //moneyText.text = "Money: " + SomeManager.SomeMoneyVariable.ToString();
        }

        public void EnableJewelImage() //OnJewelCollected event?
        {
            //jewelImage.enabled = true;
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
