using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;


namespace Zhamanta
{
    public class GlobalUI : NetworkBehaviour
    {
        [SerializeField] TMP_Text alertnessText;
        [SerializeField] Image targetImage;
        [SerializeField] GameObject lightsOffPanel;
        [SerializeField] GameObject gameOverPanel;

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


        private void Start()
        {
            targetImage.enabled = false;
            lightsOffPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            fadeImage.enabled = false;
            alarmOn = false;
        }



        [ContextMenu("EnableGameOverPanel")]
        public void EnableGameOverPanel() // CALL THIS through event
        {
            GameOverPanelCallFunctionForClientsRpc();
        }

        [Rpc(SendTo.Server)]
        public void GameOverPanelCallFunctionForClientsRpc()
        {
            EnableGameOverPanelRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void EnableGameOverPanelRpc()
        {
            gameOverPanel.SetActive(true);
        }



        [ContextMenu("EnableTargetImage")]
        public void UpdateAlertnessText() // CALL THIS (along with UpdateAlertnessLevel(float alertnessAmount))
        {
            UpdateAlertnessCallFunctionForClientsRpc();
        }

        [Rpc(SendTo.Server)]
        public void UpdateAlertnessCallFunctionForClientsRpc()
        {
            UpdateAlertnessTextRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void UpdateAlertnessTextRpc()
        {
            alertnessText.text = "Alertness: " + AlertnessLevel.alertnessL.ToString();
        }

 

        [ContextMenu("EnableTargetImage")]
        public void EnableTargetImage() // CALL THIS.  If one player collects the target valuable, everyone should see it so that they all know they can win now!
        {
            EnableTargetImageCallFunctionForClientsRpc();
        }

        [Rpc(SendTo.Server)]
        public void EnableTargetImageCallFunctionForClientsRpc()
        {
            EnableTargetImageRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void EnableTargetImageRpc()
        {
            targetImage.enabled = true;
        }



        [ContextMenu("DisableTargetImage")]
        public void DisableTargetImage() // CALL THIS.  If the player holding the target dies/gets arrested before escaping, they should drop the target valuable, so the image will disappear.
        {
            DisableTargetImageCallFunctionForClientsRpc();
        }

        [Rpc(SendTo.Server)]
        public void DisableTargetImageCallFunctionForClientsRpc()
        {
            DisableTargetImageRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void DisableTargetImageRpc() 
        {
            targetImage.enabled = false;
        }



        [ContextMenu("AlarmSwitch")]
        public void AlarmSwitch(bool alarmOn)
        {
            AlarmCallFunctionForClientsRpc(alarmOn);
        }

        [Rpc(SendTo.Server)]
        public void AlarmCallFunctionForClientsRpc(bool alarmOn)
        {
            AlarmSwitchRpc(alarmOn);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void AlarmSwitchRpc(bool alarmOn)
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



        [ContextMenu("TurnLightsOff")]
        public void TurnLightsOff()
        {
            LightsCallFunctionForClientsRpc();
        }

        [Rpc(SendTo.Server)]
        public void LightsCallFunctionForClientsRpc()
        {
            TurnLightsOffRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void TurnLightsOffRpc() 
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
