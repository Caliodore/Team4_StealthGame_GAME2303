using UnityEngine;

public class DictRefStruct : MonoBehaviour
{
    /// <summary>
    /// Concept for a single struct to put into dictionaries or arrays to tie to a singular gameObject so we can more easily compare references.
    /// </summary>
    public struct DictRef
    { 
        private string name;

        public string Name
        {
            get { return name; }
        }    


    }
}
