using Dawud.BT.Enums;
using Dawud.BT.General;
using UnityEngine;

namespace Dawud.BT.Actions
{
    /// <summary>
    /// Generic Methods for calling via BT or leaf Node.
    /// </summary>
    public class GenericActions
    {
        /// <summary>
        /// Sends the Agent to a location via Nav Mesh. Sets the Agents current status <see cref="AgentStatusEnum"/> and returns the current status <see cref="ProcessStatusEnum"/> of Leaf Node that called this method. This method sets the current <see cref="Node.Status"/> of the Leaf Node that called it.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="gameObject"></param>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static ProcessStatusEnum GoToDestination(Vector3 destination, GameObject gameObject, NPCMain npc)
        {
            float distanceToTarget = Vector3.Distance(destination, gameObject.transform.position);
            if (npc.AgentStatus.Equals(AgentStatusEnum.IDLE))
            {
                npc.Agent.SetDestination(destination);
                npc.AgentStatus = AgentStatusEnum.WORKING;
            }
            else if(Vector3.Distance(npc.Agent.pathEndPosition, destination) >= 2)
            {
                npc.AgentStatus = AgentStatusEnum.IDLE;
                return ProcessStatusEnum.FAILED;
            }
            else if(distanceToTarget < 2)
            {
                npc.AgentStatus = AgentStatusEnum.IDLE;
                return ProcessStatusEnum.SUCCESS;
            }
            return ProcessStatusEnum.RUNNING;
        }
    }
}
