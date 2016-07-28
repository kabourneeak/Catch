using System;
using System.Runtime.Serialization;

namespace MapWriter.Models
{
    [Serializable]
    public class EmitScriptEntryModel
    {
        /// <summary>
        /// The time, in ticks, when the emit order starts
        /// </summary>
        [DataMember]
        public int BeginTime { get; set; }

        /// <summary>
        /// The number of agents to emit
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        /// <summary>
        /// The time to delay emission between agents
        /// </summary>
        [DataMember]
        public int DelayTime { get; set; }

        /// <summary>
        /// The path to set the agents upon
        /// </summary>
        [DataMember]
        public string PathName { get; set; }

        /// <summary>
        /// The type of agent to emit
        /// </summary>
        [DataMember]
        public string AgentTypeName { get; set; }
    }
}