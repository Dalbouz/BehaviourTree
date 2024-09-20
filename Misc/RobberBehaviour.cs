using UnityEngine;
using Dawud.BT.General;
using Dawud.BT.Enums;
using Dawud.BT.Actions;
using System.Collections.Generic;
using Dawud.BT.Misc;

namespace Dawud.BT.Behaviour
{
    /// <summary>
    /// 
    /// </summary>
    public class RobberBehaviour : NPCRoot
    {
        private int _currentItemStealing = -1;
        private GameObject _atCurrentDoor = default;
        private BTPrioritySelector _goToDoorPrioritySel = default;

        [SerializeField] private GameObject _van = default;
        [SerializeField] private GameObject _backDoor = default;
        [SerializeField] private GameObject _frontDoor = default;
        [SerializeField, Range(0, 2000)] private int _money = 800;
        [SerializeField, Range(0, 2000)] private int _amountOfMoneyNeeded = 800;
        [SerializeField] private List<ItemData> _collectedItems = new List<ItemData>();
        [SerializeField] private List<ItemGeneric> _itemsToSteal = new List<ItemGeneric>();

        protected override void Start()
        {
            _currentItemStealing = -1;
            _collectedItems = new List<ItemData>();
            _itemsToSteal = new List<ItemGeneric>();
            _itemsToSteal.AddRange(GenericActions.RandomAddingPickupableItemsToList());

            base.Start();
        }

        protected override void CreateBehaviour()
        {
            BTRepeatedSequence stealSeq = new BTRepeatedSequence("Steal (Repeated Sequence: " + _itemsToSteal.Count + ")", _itemsToSteal.Count);
            BTInverter hasGotMoneyInvert = new BTInverter("Has got money(Inverter)");
            BTSequence checkFrontDoorStatusSeq = new BTSequence("Check is Front Door Unlocked(sequence)", 1);
            BTSequence checkBackDoorStatusSeq = new BTSequence("Check is Back Door Unlocked(sequence)", 2);
            _goToDoorPrioritySel = new BTPrioritySelector("Go To Door(Priority slector)");
            BTSequence stealItemSeq = new BTSequence("Steal item(Sequence)");

            BTLeaf gotMoney = new BTLeaf("Got Money", GotMoney);
            BTLeaf checkDoorStatus = new BTLeaf("Check Door Status", CheckDoorStatus);
            BTLeaf goToFrontDoor = new BTLeaf("Go To Front Door", GoToFrontDoor);
            BTLeaf goToBackDoor = new BTLeaf("Go To back door", GoToBackDoor);
            BTLeaf goToItem = new BTLeaf("Go To Item", GoToItem);
            BTLeaf stealItem = new BTLeaf("Steal Item ", StealItem);
            BTLeaf goToVan = new BTLeaf("Go To Van", GoToVan);

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

            if (_tree.Children.Count <= 0)
            {
                _tree.AddChildren(stealSeq);
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
            return GenericActions.GoToDestination(_itemsToSteal[_currentItemStealing + 1].gameObject, this);
        }

        private ProcessStatusEnum StealItem()
        {
            ProcessStatusEnum status = GenericActions.StealItem(this, _itemsToSteal[_currentItemStealing + 1].gameObject);

            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                ItemData id = GenericActions.GetItemData(_itemsToSteal[_currentItemStealing + 1].gameObject);
                if (id != null && id.Pickupable)
                {
                    _collectedItems.Add(id); // Add to the list if the component exists
                }

                _currentItemStealing++;
            }
            return status;
        }

        private ProcessStatusEnum GoToVan()
        {
            ProcessStatusEnum status = GenericActions.GoToDestination(_van, this);
            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                _money += _collectedItems[_currentItemStealing].Value;
                _itemsToSteal[_currentItemStealing].gameObject.SetActive(false);
                _itemsToSteal[_currentItemStealing].gameObject.transform.parent = null;

                if (_currentItemStealing + 1 >= _itemsToSteal.Count)
                {
                    _collectedItems.Clear();
                    _itemsToSteal.Clear();
                    _currentItemStealing = -1;
                }
            }
            return status;
        }

        private ProcessStatusEnum GoToFrontDoor()
        {
            _atCurrentDoor = _frontDoor;
            return GenericActions.GoToDoor(_frontDoor, this);
        }

        private ProcessStatusEnum GoToBackDoor()
        {
            _atCurrentDoor = _backDoor;
            return GenericActions.GoToDoor(_backDoor, this);
        }

        private ProcessStatusEnum CheckDoorStatus()
        {
            ProcessStatusEnum doorStatus = GenericActions.CheckDoorStatus(_atCurrentDoor.GetComponent<DoorLock>());
            if (doorStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                if(_goToDoorPrioritySel.Children[_goToDoorPrioritySel.CurrentChild].SortOrder != 1) //change the priority so that the unlocked door is the first one to go through.
                {
                    foreach (var item in _goToDoorPrioritySel.Children)
                    {
                        item.SortOrder += 1;
                    }
                    _goToDoorPrioritySel.Children[_goToDoorPrioritySel.CurrentChild].SortOrder = 1;
                }
                //_atCurrentDoor.GetComponent<DoorMovement>().StartMoveUpCoroutine();
                _atCurrentDoor.SetActive(false);
                _atCurrentDoor = null;
                return doorStatus;
            }
            _atCurrentDoor = null;
            return doorStatus;
        }
    }
}
