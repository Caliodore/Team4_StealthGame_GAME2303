using UnityEngine;

public class AlertnessLevel : MonoBehaviour
{
    public static float alertnessL;

    public void IncreaseAlertnessLevel(float alertnessAmount, int eventType)
    {
        alertnessL += alertnessAmount;
    }
}
