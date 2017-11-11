using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Alta.INetwork
{
    /// <summary>
    /// Network socket client
    /// </summary>
    public class Client:IClient
    {
        /// <summary>
        /// UUID of client
        /// </summary>
        public string Code
        {
            get
            {
                return this.player.guid;
            }
        }
        /// <summary>
        /// Info of client
        /// </summary>
        public NetworkPlayer player
        {
            get;
            set;
        }
        /// <summary>
        /// Kind of client
        /// </summary>

        public TypeClient Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        private TypeClient type = TypeClient.None;
    }
}
