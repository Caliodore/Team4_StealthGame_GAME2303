using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectorOptions", menuName = "Director/Options")]

// Contains varibales that change up the decision making of the DirectorAI
// Each level can have its own DirectorOptions
public class DirectorOptions : ScriptableObject
{
    public float lockAllDoorsMinTime = 5f;
    public float lockAllDoorsMaxTime = 10f;
}
