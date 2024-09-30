using UnityEngine;
using Dawud.BT.General;
using Dawud.BT.Enums;
using Dawud.BT.Actions;
using System.Collections.Generic;
using Dawud.BT.Misc;

namespace Dawud.BT.Behavior
{
    /// <summary>
    /// 
    /// </summary>
    public class RobberBehavior : NPCRoot
    {
        private int _currentItemStealing = -1;
        private GameObject _usingDoor = default;
        private BTPrioritySelector _goToDoorPrioritySel = default;
        private BTRepeatNodeFromIndex _repeateRunAwaySeq = default;

        [SerializeField] private GameObject _van = default;
        [SerializeField] private GameObject _backDoor = default;
        [SerializeField] private GameObject _frontDoor = default;
        [SerializeField, Range(0, 2000)] private int _money = 800;
        [SerializeField, Range(0, 2000)] private int _amountOfMoneyNeeded = 800;
        [SerializeField] private ItemData _collectedItem = default;
        [SerializeField] private List<ItemGeneric> _itemsToSteal = new List<ItemGeneric>();

        protected override void Start()
        {
            _currentItemStealing = -1;
            _itemsToSteal = new List<ItemGeneric>();
            _itemsToSteal.AddRange(GenericActions.RandomAddingPickupableItemsToList());

            base.Start();
        }

        protected override void CreateBehavior()
        {
            BTRepeatedSequence stealSeq = new BTRepeatedSequence("Steal (Repeated Sequence: " + _itemsToSteal.Count + ")", _itemsToSteal.Count);
            BTInverter hasGotMoneyInvert = new BTInverter("Has got money(Inverter)");
            BTSequence checkFrontDoorStatusSeq = new BTSequence("Check is Front Door Unlocked(sequence)", 1);
            BTSequence checkBackDoorStatusSeq = new BTSequence("Check is Back Door Unlocked(sequence)", 2);
            _goToDoorPrioritySel = new BTPrioritySelector("Go To Door(Priority selector)");
            BTSequence stealItemSeq = new BTSequence("Steal item(Sequence)");
            BTRepeatNodeFromIndex returnBTToMainNode = new BTRepeatNodeFromIndex("Return to Main Node", _tree, 0, true);
            BTSequence runAwaySeq = new BTSequence("Run Away (Sequence)");
            _repeateRunAwaySeq = new BTRepeatNodeFromIndex("Return Run Away Node to start", runAwaySeq, 0, true);

            BTLeaf gotMoney = new BTLeaf("Got Money", GotMoney);
            BTLeaf checkDoorStatus = new BTLeaf("Check Door Status", CheckDoorStatus);
            BTLeaf goToFrontDoor = new BTLeaf("Go To Front Door", GoToFrontDoor);
            BTLeaf goToBackDoor = new BTLeaf("Go To back door", GoToBackDoor);
            BTLeaf goToItem = new BTLeaf("Go To Item", GoToItem);
            BTLeaf stealItem = new BTLeaf("Steal Item ", StealItem);
            BTLeaf goToVan = new BTLeaf("Go To Van", GoToVan);
            BTLeaf runAway = new BTLeaf("Run Away", RunAway);
            BTLeaf checkIsChased = new BTLeaf("Check is Chased", CheckIfStillChased);

            hasGotMoneyInvert.AddChildren(gotMoney);

            checkFrontDoorStatusSeq.AddChildren(goToFrontDoor);
            checkFrontDoorStatusSeq.AddChildren(checkDoorStatus);

            checkBackDoorStatusSeq.AddChildren(goToBackDoor);
            checkBackDoorStatusSeq.AddChildren(checkDoorStatus);

            _goToDoorPrioritySel.AddChildren(checkFrontDoorStatusSeq);
            _goToDoorPrioritySel.AddChildren(checkBackDoorStatusSeq);

            stealItemSeq.AddChildren(goToItem);
            stealItemSeq.AddChildren(stealItem);

            stealSeq.AddChildren(hasGotMoneyInvert);
            stealSeq.AddChildren(_goToDoorPrioritySel);
            stealSeq.AddChildren(stealItemSeq);
            stealSeq.AddChildren(goToVan);

            runAwaySeq.AddChildren(runAway);
            runAwaySeq.AddChildren(checkIsChased);
            runAwaySeq.AddChildren(_repeateRunAwaySeq);
            runAwaySeq.AddChildren(returnBTToMainNode);

            if (_tree.Children.Count <= 0)
            {
                _tree.AddChildren(stealSeq);
                _tree.AddChildren(runAwaySeq);
            }
        }

        private ProcessStatusEnum GotMoney()
        {
            if (_money >= _amountOfMoneyNeeded)
            {
                return ProcessStatusEnum.SUCCESS;
            }
            return ProcessStatusEnum.FAILED;
        }

        private ProcessStatusEnum GoToItem()
        {
            return GenericActions.GoToDestination(_itemsToSteal[_currentItemStealing + 1].gameObject.transform.position, this);
        }

        private ProcessStatusEnum StealItem()
        {
            ProcessStatusEnum status = GenericActions.StealItem(this, _itemsToSteal[_currentItemStealing + 1].gameObject);

            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                ItemData id = GenericActions.GetItemData(_itemsToSteal[_currentItemStealing + 1].gameObject);
                if (id != null && id.Pickupable)
                {
                    _collectedItem = id; // Add to the list if the component exists
                }

                _currentItemStealing++;
            }
            return status;
        }

        private ProcessStatusEnum GoToVan()
        {
            ProcessStatusEnum status = GenericActions.GoToDestination(_van.transform.position, this);
            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                _money += _collectedItem.Value;
                _itemsToSteal[_currentItemStealing].gameObject.SetActive(false);
                _itemsToSteal[_currentItemStealing].gameObject.transform.parent = null;

                _collectedItem = null;

                if (_currentItemStealing + 1 >= _itemsToSteal.Count)
                {
                    _itemsToSteal.Clear();
                    _currentItemStealing = -1;
                }
            }
            return status;
        }

        private ProcessStatusEnum GoToFrontDoor()
        {
            _usingDoor = _frontDoor;
            return GenericActions.GoToDestination(_frontDoor.transform.position, this);
        }

        private ProcessStatusEnum GoToBackDoor()
        {
            _usingDoor = _backDoor;
            return GenericActions.GoToDestination(_backDoor.transform.position, this);
        }

        private ProcessStatusEnum CheckDoorStatus()
        {
            ProcessStatusEnum doorStatus = GenericActions.CheckDoorStatus(_usingDoor.GetComponent<DoorLock>());

            if (doorStatus.Equals(ProcessStatusEnum.SUCCESS))//if the door is unlocked, it will set that door to be no. 1 priority.
            {
                if(_goToDoorPrioritySel.Children[_goToDoorPrioritySel.CurrentChild].SortOrder != 1) //change the priority so that the unlocked door is the first one to go through.
                {
                    foreach (var item in _goToDoorPrioritySel.Children)
                    {
                        item.SortOrder += 1;//Set every Node to one priority higher
                    }
                    _goToDoorPrioritySel.Children[_goToDoorPrioritySel.CurrentChild].SortOrder = 1;//Next set the current door that returned SUCCESS to no.1 priority
                }
                //_atCurrentDoor.GetComponent<DoorMovement>().StartMoveUpCoroutine();
                _usingDoor.SetActive(false);
                //_atCurrentDoor = null;
                return doorStatus;
            }
            _usingDoor = null;
            return doorStatus;
        }

        protected override void SawOtherAgent()
        {
            //foreach (NPCRoot cop in SeenAgents)
            //{
            //    if(cop.SeenAgents[0] == this)
            //    {
                    _tree.JumpToNode(1);
            //        break;
            //    }
            //}
        }

        private ProcessStatusEnum CheckIfStillChased()
        {
            bool isChased = false;
            int numOfFoundCops = 0;
            foreach (NPCRoot cop in SeenAgents)
            {
                if (cop.SeenAgents[0] == this)
                {
                    numOfFoundCops++;
                    break;
                }
            }

            if(numOfFoundCops >= 1)
            {
                isChased = true;
            }
            _repeateRunAwaySeq.IsConditionMeet = isChased;
           
            return ProcessStatusEnum.SUCCESS;
        }

        private ProcessStatusEnum RunAway()
        {
            ProcessStatusEnum status = ProcessStatusEnum.RUNNING;

            NPCRoot closestCop = null;
            float smallestCopRobberDistance = 100000f;
            foreach (NPCRoot cop in SeenAgents)
            {
                float checkDistance = Vector3.Distance(cop.transform.position, this.transform.position);
                if (checkDistance < smallestCopRobberDistance)
                {
                    smallestCopRobberDistance = checkDistance;
                    closestCop = cop;
                }
            }

            if (_collectedItem != null)
            {
                float distanceToOpenDoor = Vector3.Distance(_usingDoor.transform.position, this.transform.position);
                
                if(distanceToOpenDoor <= smallestCopRobberDistance)
                {
                    return GoToVan();
                }
            }
            else
            {
                status = GenericActions.GoToDestination(transform.position + (transform.position -closestCop.transform.position).normalized * (_seeDistance * 2), this);
            }
            return status;
        }
    }
}
