using Dawud.BT.Enums;
using Dawud.BT.General;
using UnityEngine;

namespace Dawud.BT.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericActions
    {
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
