using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Cali;


namespace Zhamanta
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

        // Could use UnityActions/Events instead of Update() for optimization. Cali: highly agree
        private void Update()
        {
            UpdateHealthBar();
            UpdateAlertnessText();
            UpdateMoneyCountText();
        }
        public void UpdateHealthBar()
        {
            //healthBarImage.fillAmount = playerStats.currentHealth / playerStats.maxhealth;
            healthBarImage.fillAmount = playerStats.health / 100;
        }

        public void UpdateAlertnessText()
        {
            alertnessText.text = "Alertness: " + AlertnessLevel.alertnessL.ToString();
        }

        public void UpdateMoneyCountText() // Dedicated event
        {
            // Update money text
        }

        public void EnableJewelImage() // Dedicated event
        {
            // Enable Jewel Image
        }

        public void FlashingRed() // Cannot be called on an event because lerp needs Update(), DirectorAI will call this
        {
            // Make screen alternate between light and dark red gradually using lerp
        }

        public void TurnOffLights() // Cannot be called on an event because lerp needs Update(), DirectorAI will call this
        {
            // Make screen darker gradually using lerp
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
