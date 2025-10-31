using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Cali
{
    

    /// <summary>
    /// Initialize this struct with a script reference attached to an object and assign an index number.<br/>
    /// Then, using the logic script attached to the object, you can find a specific GameObjRef and any attached info without GetComponent.
    /// </summary>
    public struct DictRef
    {
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

        /// <summary>
        /// Default declaration for the VALUES of our centralized dictionary, associated to a group key.<br/>
        /// For clarification: group index is the VALUE's specific index WITHIN the KEY group it is associated with.
        /// </summary>
        /// <param name="attachedScriptRef">The MonoBehaviour script that is used as an identifier.</param>
        /// <param name="groupIndexInput">The user-assigned index of this object within its group.</param>
        public DictRef(MonoBehaviour attachedScriptRef, int groupIndexInput)
        {
            groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
            refGroup = groupType.ObjScriptName;
            groupIndex = groupIndexInput;
            objName = groupType.ObjReference.name;
            objRef = groupType.ObjReference;
        }

        /// <summary>
        /// Single-string input is used for generating default values at an index of -99 based on strings for the group tags.
        /// </summary>
        /// <param name="tagInput"></param>
        public DictRef(string tagInput)
        {
            MonoBehaviour attachedScriptRef;
            switch (tagInput) 
            {
                case("Player"):
                    attachedScriptRef = new PlayerLogic();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                case("Guard"):
                    attachedScriptRef = new GuardLogic();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                case("Valuable"):
                    attachedScriptRef = new Valuable();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                case("Door"):
                    attachedScriptRef = new DoorLogic();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                case("Exit"):
                    attachedScriptRef = new Exit();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                case("Room"):
                    attachedScriptRef = new Room();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                case("RestrictedArea"):
                    attachedScriptRef = new RestrictedArea();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;

                default:
                    attachedScriptRef = new DRefDefault();
                    groupType = new ObjectGroup<MonoBehaviour>(attachedScriptRef);
                    break;
            }
            groupIndex = -99;
            refGroup = groupType.ObjScriptName;
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

}