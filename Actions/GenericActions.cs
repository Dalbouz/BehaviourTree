using Dawud.BT.Behavior;
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
        /// Generic method that Sends the Agent to a location via Nav Mesh. Sets the Agents current status <see cref="AgentStatusEnum"/> and returns the current status <see cref="ProcessStatusEnum"/> of Leaf Node that called this method. This method sets the current <see cref="BTNode.Status"/> of the Leaf Node that called it.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static ProcessStatusEnum GoToDestination(Vector3 destination, NPCRoot npc)
        {
            float distanceToTarget = Vector3.Distance(destination, npc.transform.position);
            if (npc.AgentStatus.Equals(AgentStatusEnum.IDLE))//If the agent is in idle start the SetDestination and set Agent status to Working
            {
                npc.Agent.SetDestination(destination);
                npc.AgentStatus = AgentStatusEnum.WORKING;
            }
            else if(Vector3.Distance(npc.Agent.pathEndPosition, destination) >= 2)//If the agents distance between its end path and the destination is higher or equal then 2 the agent has failed to get to the destination
            {
                npc.AgentStatus = AgentStatusEnum.IDLE;
                return ProcessStatusEnum.FAILED;
            }
            else if(distanceToTarget < 2)//If the agents distance to the destination is less then 2 the agent arrived to the destination. Set the Agent status to Idle and check what item did the agent reach. Return a value depending on the item that the Agent has reached.
            {
                npc.AgentStatus = AgentStatusEnum.IDLE;
                return ProcessStatusEnum.SUCCESS;
            }
            return ProcessStatusEnum.RUNNING;
        }

        /// <summary>
        /// Generic method for checking is the Door locked. Returns <see cref="ProcessStatusEnum"/>
        /// </summary>
        /// <param name="doorLock"></param>
        /// <returns></returns>
        public static ProcessStatusEnum CheckDoorStatus(DoorLock doorLock)
        {
            if (!doorLock.IsDoorLocked)
            {
                return ProcessStatusEnum.SUCCESS;
            }
            else
            {
                return ProcessStatusEnum.FAILED;
            }
        }

        /// <summary>
        /// Generic method for getting the <see cref="ItemData"/> from a given gameobject.
        /// </summary>
        /// <param name="gameobject"></param>
        /// <returns></returns>
        public static ItemData GetItemData(GameObject gameobject)
        {
            if (gameobject.TryGetComponent<ItemRoot>(out ItemRoot itemRoot)) // Try to get the component and check for null
            {
                return itemRoot.Data; // Add to the list if the component exists
            }
            return null;
        }

        /// <summary>
        /// Generic method for returning a random list of pick-up items form <see cref="ItemManager.PickupableItems"/>.
        /// </summary>
        /// <returns></returns>
        public static List<ItemGeneric> RandomAddingPickupableItemsToList()
        {
            List<ItemGeneric> list = new List<ItemGeneric>();
            int numbOfItemsRnd = Random.Range(1, ItemManager.Instance.PickupableItems.Count + 1);
            Debug.Log("Number of random Items: " + numbOfItemsRnd);

            List<int> rndIndexes = new List<int>();
            int rndIndex = 0;
            for (int i = 0; i < numbOfItemsRnd; i++)
            {
                do
                {
                    rndIndex = Random.Range(0, ItemManager.Instance.PickupableItems.Count);
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

        /// <summary>
        /// Generic method for returning a random list of pick-up items form <see cref="ItemManager.PickupableItems"/>.
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> RandomShuffleGameObjectList(List<GameObject> givenList)
        {
            List<GameObject> list = new List<GameObject>();

            List<int> rndIndexes = new List<int>();
            int rndIndex = 0;
            for (int i = 0; i < givenList.Count; i++)
            {
                do
                {
                    rndIndex = Random.Range(0, givenList.Count);
                }
                while (rndIndexes.Contains(rndIndex));

                Debug.Log("Random index: " + rndIndex);
                rndIndexes.Add(rndIndex);
            }

            for (int i = 0; i < rndIndexes.Count; i++)
            {
                list.Add(givenList[rndIndexes[i]]);
            }
            return list;
        }

        /// <summary>
        /// Set the <paramref name="item"/> gameobject as a child of the <paramref name="npc"/> gameobject. Return SUCCESS or FAILED.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ProcessStatusEnum StealItem(NPCRoot npc, GameObject item)
        {
            float distance = Vector3.Distance(npc.transform.position, item.transform.position);

            if(distance <= 2)
            {
                item.transform.parent = npc.gameObject.transform;
                return ProcessStatusEnum.SUCCESS;
            }
            return ProcessStatusEnum.FAILED;
        }
    }
}
