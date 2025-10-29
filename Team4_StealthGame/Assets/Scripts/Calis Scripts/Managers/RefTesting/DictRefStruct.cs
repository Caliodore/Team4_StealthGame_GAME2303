using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Cali
{
    //namespace DRef
    //{ 

    /// <summary>
    /// Just some testing at making a struct that cand hold a simple class for taking in an attached MonoBehaviour and determining how to group it.
    /// </summary>
    //public class DictRefStruct : MonoBehaviour
    //{
        /// <summary>
        /// Concept for a struct to use when searching for an object in a universal dictionary.
        /// Initialize it with a script reference attached to an object and assign an index number.
        /// Then, using the script name reference in the dictionary, you can find a specific GameObjRef and any attached info without GetComponent.
        /// </summary>
        public struct DictRef
        {
            //Custom class for taking in MonoBehaviour scripts and outputting name of script and the name of the object using it.
            private readonly ObjectGroup<MonoBehaviour> groupType;
            public ObjectGroup<MonoBehaviour> GroupType { get { return groupType; } }

            private readonly string refGroup;
            public string RefGroup { get { return refGroup; } }

            private readonly int groupIndex;
            public int GroupIndex { get { return groupIndex; } }

            private readonly string objName;
            public string ObjName { get { return objName; } }

            private readonly GameObject objRef;
            public GameObject ObjRef { get { return objRef; } }

            public DictRef(MonoBehaviour attachedScriptRef, int groupIndexInput)
            {
                groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                refGroup = groupType.ObjScriptName;
                groupIndex = groupIndexInput;
                objName = groupType.ObjReference.name;
                objRef = groupType.ObjReference;
            }

            public override string ToString()
            {
                return new string($"GroupType: {groupType.ToString()}, RefGroup: {refGroup}, Group Index: {groupIndex.ToString()}, ObjName: {objName}");
            }
        }

        public class ObjectGroup<T> where T : MonoBehaviour
        {
            private T objType;
            private string objScriptName;
            private GameObject gameObjRef;

            public ObjectGroup(T objTypeInput)
            {
                objType = objTypeInput;
                objScriptName = objType.GetType().Name;
                gameObjRef = objTypeInput.gameObject;
                if (gameObjRef == null)
                {
                    gameObjRef = objType.GetComponentInParent<Transform>().gameObject;
                    if (gameObjRef == null)
                        MonoBehaviour.print($"This Object Group is not attached to a GameObject.");
                }
            }

            public string ObjScriptName
            {
                get { return objScriptName; }
            }

            public GameObject ObjReference
            {
                get { return gameObjRef; }
            }

            public override string ToString()
            {
                return objScriptName;
            }

        }
    //}
//}
}