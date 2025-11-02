using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Cali;


namespace Zhamanta
{
    public class GlobalUI : MonoBehaviour
    {
        [SerializeField] TMP_Text alertnessText;
        [SerializeField] Image targetImage;

        // Alarm UI
        private bool alarmOn;
        [SerializeField]
        private Image fadeImage;
        [SerializeField]
        Color targetColor1;
        [SerializeField]
        Color targetColor2;
        [SerializeField]
        float fadeSpeed;
        Color currentTarget;

        // Lights off UI
        [SerializeField]
        private GameObject lightsOffPanel;

        private void Start()
        {
            targetImage.enabled = false;
            lightsOffPanel.SetActive(false);
            fadeImage.enabled = false;
            alarmOn = false;
        }

        // Could use UnityActions/Events instead of Update() for optimization. Cali: highly agree
        private void Update()
        {
            UpdateAlertnessText();
        }


        public void UpdateAlertnessText()
        {
            alertnessText.text = "Alertness: " + AlertnessLevel.alertnessL.ToString();
        }

        public void EnableTargetImage() // If one player collects the target valuable, everyone should see it so that they all know they can win now!
        {
            targetImage.enabled = true;
        }

        public void DisableTargetImage() // If the player holding the target dies/gets arrested before escaping, they should drop the target valuable.
        {
            targetImage.enabled = false;
        }

        public void AlarmSwitch(bool alarmOn)
        {
            if (alarmOn)
            {
                fadeImage.enabled = true;
            }
            else
            {
                fadeImage.enabled = false;  
            }
        }

        public void FlashingRed() // Cannot be called on an event because lerp needs Update(), DirectorAI will call this
        {
            var currentColor = fadeImage.color;

            if (currentTarget == targetColor1)
            {
                currentColor = Color.Lerp(currentColor, targetColor1, fadeSpeed * Time.deltaTime);
                fadeImage.color = currentColor;
                if (currentColor == targetColor1)
                {
                    currentTarget = targetColor2;
                }

            }

            if (currentTarget == targetColor2)
            {
                currentColor = Color.Lerp(currentColor, targetColor2, fadeSpeed * Time.deltaTime);
                fadeImage.color = currentColor;
                if (currentColor == targetColor2)
                {
                    currentTarget = targetColor1;
                }

            }
        }

        public void TurnLightsOff() 
        {
            StartCoroutine(LightsOffDuration());
        }

        IEnumerator LightsOffDuration()
        {
            lightsOffPanel.SetActive(true);
            yield return new WaitForSeconds(15);
            lightsOffPanel.SetActive(false);
        }
    }
}
