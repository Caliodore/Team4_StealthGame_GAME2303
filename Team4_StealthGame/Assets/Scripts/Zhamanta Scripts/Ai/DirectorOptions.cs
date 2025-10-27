using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectorOptions", menuName = "Director/Options")]

// Contains varibales that change up the decision making of the DirectorAI
// Each level can have its own DirectorOptions
public class DirectorOptions : ScriptableObject
{
    public float lockdownMinTime = 10f;
    public float lockdownMaxTime = 20f;

    public int stage1MaxGuards = 10;
    public int stage2MaxGuards = 20;
    public int alarmMaxGuards = 30;
    public int lockdownMaxGuards = 40;

    public float guardSpeedAlarmMultiplier = 1.5f;
    public float guardSpeedLockdownMultiplier = 2f;
}
