using Dawud.BT.Behaviour;
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
        /// <param name="destinationObject"></param>
        /// <param name="gameObject"></param>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static ProcessStatusEnum GoToDestination(GameObject destinationObject, NPCMain npc, ItemEnum itemDestinaton)
        {
            float distanceToTarget = Vector3.Distance(destinationObject.transform.position, npc.transform.position);
            if (npc.AgentStatus.Equals(AgentStatusEnum.IDLE))//If the agent is in idle start the SetDestination and set Agent status to Working
            {
                npc.Agent.SetDestination(destinationObject.transform.position);
                npc.AgentStatus = AgentStatusEnum.WORKING;
            }
            else if(Vector3.Distance(npc.Agent.pathEndPosition, destinationObject.transform.position) >= 2)//If the agents distance betwen its endpath and the desitnation is higher or equal then 2 the agent has failed to get to the destination
            {
                npc.AgentStatus = AgentStatusEnum.IDLE;
                return ProcessStatusEnum.FAILED;
            }
            else if(distanceToTarget < 2)//If the agents distance to the destination is less then 2 the agent arrived to the destination. Set the Agent status to Idle and check what item did the agent reach. Return a value depending on the item that the Agent has reached.
            {
                npc.AgentStatus = AgentStatusEnum.IDLE;
                return CheckItemDestination(itemDestinaton, destinationObject);
            }
            return ProcessStatusEnum.RUNNING;
        }


        public static ProcessStatusEnum CheckItemDestination(ItemEnum itemDestination, GameObject gameObject)
        {
            switch (itemDestination)
            {
                case ItemEnum.NONE:
                    return ProcessStatusEnum.SUCCESS;

                case ItemEnum.DOOR:
                    if (!gameObject.GetComponent<DoorLock>().IsDoorLocked)
                    {
                        return ProcessStatusEnum.SUCCESS;
                    }
                    else
                    {
                        return ProcessStatusEnum.FAILED;
                    }

                case ItemEnum.DIAMOND:
                    return ProcessStatusEnum.SUCCESS;
            }

            return ProcessStatusEnum.SUCCESS;
        }
    }
}
