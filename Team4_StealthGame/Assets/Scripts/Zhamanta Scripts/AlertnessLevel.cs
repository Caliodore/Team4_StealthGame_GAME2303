using UnityEngine;

namespace Zhamanta
{
    public class AlertnessLevel : MonoBehaviour
    {
        public static float alertnessL;

        private void Start()
        {
            alertnessL = 0;
        }
        public static void UpdateAlertnessLevel(float alertnessAmount)
        {
            alertnessL += alertnessAmount;
        }
    }
}
