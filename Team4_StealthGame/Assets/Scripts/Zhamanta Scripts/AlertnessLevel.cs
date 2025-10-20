using UnityEngine;

public class AlertnessLevel : MonoBehaviour
{
    public static float alertnessL;

    private void Start()
    {
        alertnessL = 0;
    }
    public void IncreaseAlertnessLevel(float alertnessAmount)
    {
        alertnessL += alertnessAmount;
    }
}
