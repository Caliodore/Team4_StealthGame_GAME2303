using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Testing
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] PlayerStats playerStats;
        [SerializeField] TMP_Text moneyText;
        [SerializeField] TMP_Text alertnessText;
        [SerializeField] Image healthBarImage;
        //[SerializeField] Image jewelImage;

        private void Start()
        {
            //jewelImage.enabled = false;
        }

        // Could use UnityActions/Events instead of Update() for optimization
        private void Update()
        {
            UpdateHealth();
            UpdateAlertness();
            UpdateMoneyCount();
        }
        public void UpdateHealth() // Onhurt event?
        {
            //healthBarImage.fillAmount = playerStats.currentHealth / playerStats.maxhealth;
            healthBarImage.fillAmount = playerStats.health / 100;
        }

        public void UpdateAlertness() // OnAlertnessUpdated event?
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
    }
}
