using JetBrains.Annotations;
using UnityEngine;

namespace Cali
{
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] PlayerStats playerStatsRef;
        [SerializeField] bool isTrespassing;

        /// <summary>
        /// Should guards be alerted by hearing/seeing this player?
        /// </summary>
        public bool IsTrespassing
        {
            get { return isTrespassing; }
            set { isTrespassing = value; }
        }

        public PlayerBase()
        {

        }
    }
}