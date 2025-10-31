using Cali;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central reference for pulling values associated with tags of objects.
/// </summary>
public class CentralRefDictionary : MonoBehaviour
{
    public List<string> tagRefs;
    public Dictionary<string, DictRef> centralDict;

    private void Awake()
    {
        GenerateTagRefs();
        GenerateCentralDict();
    }    

    /// <summary>
    /// Grabs all user-generated tags and adds 'Player' to the beginning of the list.
    /// </summary>
    private void GenerateTagRefs()
    {
        string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;
        tagRefs.Add("Player");
        for (int i = 7; i < allTags.Length; i++)
        {
            tagRefs.Add(allTags[i+1]);
        }
    }

    /// <summary>
    /// For use within the centralized dictionary, default group index is -99.
    /// </summary>
    private void GenerateCentralDict()
    { 
        foreach(string tagName in tagRefs)
        {
            DictRef defaultDR = new DictRef(tagName);
            centralDict.Add(tagName, defaultDR);
        }
    }

    public void AddToDictionary(MonoBehaviour inputScript)
    {
        int valueCount = 0;

    }
}
