using Dawud.BT.Behaviour;
using Dawud.BT.Enums;
using Dawud.BT.General;
using Dawud.BT.Misc;
using System.Collections.Generic;
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

        public static ItemData GetPickupableItemData(GameObject gameobject)
        {
            if (gameobject.TryGetComponent<ItemRoot>(out ItemRoot itemRoot)) // Try to get the component and check for null
            {
                if (itemRoot.Data.Pickupable)
                    return itemRoot.Data; // Add to the list if the component exists
            }
            return null;
        }

        public static List<GameObject> RandomAddingPickupableItemsToList()
        {
            List<GameObject> list = new List<GameObject>();
            int numbOfItemsRnd = Random.Range(1, ItemManager.Instance.PickupableItems.Count);
            Debug.Log("Number of random Items: " + numbOfItemsRnd);

            List<int> rndIndexes = new List<int>();
            int rndIndex = 0;
            for (int i = 0; i < numbOfItemsRnd; i++)
            {
                do
                {
                    rndIndex = Random.Range(0, ItemManager.Instance.PickupableItems.Count - 1);
                }
                while (rndIndexes.Contains(rndIndex));

                Debug.Log("Random index: " + rndIndex);
                rndIndexes.Add(rndIndex);
            }

            for (int i = 0; i < rndIndexes.Count; i++)
            {
                list.Add(ItemManager.Instance.PickupableItems[rndIndexes[i]]);
            }
            return list;
        }

        public static ProcessStatusEnum GoToItemToSteal(GameObject destinationObject, ItemEnum itemToSteal, NPCMain robber)
        {
            ProcessStatusEnum itemStatus = GoToDestination(destinationObject, robber, itemToSteal);
            if (itemStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                destinationObject.transform.parent = robber.gameObject.transform;
                return itemStatus;
            }
            return itemStatus;
        }


        public static ProcessStatusEnum GoToDoor(GameObject door, NPCMain npc)
        {
            ProcessStatusEnum doorStatus = GoToDestination(door, npc, ItemEnum.DOOR);
            if (doorStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                door.SetActive(false);
                return doorStatus;
            }
            return doorStatus;
        }
    }
}
