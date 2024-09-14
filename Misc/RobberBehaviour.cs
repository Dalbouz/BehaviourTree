using UnityEngine;
using Dawud.BT.General;
using UnityEngine.AI;
using Dawud.BT.Enums;
using Dawud.BT.Actions;
using System.Collections.Generic;
using Dawud.BT.Misc;
using System.Collections;

namespace Dawud.BT.Behaviour
{
    /// <summary>
    /// 
    /// </summary>
    public class RobberBehaviour : NPCMain
    {
        private int _currentItemStealing = -1;
        private GameObject _atCurrentDoor = default;

        [SerializeField] private GameObject _van = default;
        [SerializeField] private GameObject _backDoor = default;
        [SerializeField] private GameObject _frontDoor = default;
        [SerializeField, Range(0, 1000)] private int _money = 800;
        [SerializeField] private List<ItemData> _collectedItems = new List<ItemData>();
        [SerializeField] private List<ItemGeneric> _itemsToSteal = new List<ItemGeneric>();

        protected override void Start()
        {
            base.Start();

            _currentItemStealing = -1;
            _collectedItems = new List<ItemData>();
            _itemsToSteal = new List<ItemGeneric>();
            _itemsToSteal.AddRange(GenericActions.RandomAddingPickupableItemsToList());
        }

        protected override void CreateBehaviour()
        {
            Sequence stealSeq = new Sequence("Steal something(Sequence)");
            Sequence checkFrontDoorStatusSeq = new Sequence("Check is Front Door Unlocked(sequence)");
            Sequence checkBackDoorStatusSeq = new Sequence("Check is Back Door Unlocked(sequence)");
            Selector goToDoorSel = new Selector("Go To Door(slector)");
            Inverter hasGotMoneyInvert = new Inverter("Has got money(Inverter)");

            Leaf gotMoney = new Leaf("Got Money", GotMoney);
            Leaf checkDoorStatus = new Leaf("Check Door Status", CheckDoorStatus);
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf goToBackDoor = new Leaf("Go To back door", GoToBackDoor);
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            checkFrontDoorStatusSeq.AddChildren(goToFrontDoor);
            checkFrontDoorStatusSeq.AddChildren(checkDoorStatus);
            checkBackDoorStatusSeq.AddChildren(goToBackDoor);
            checkBackDoorStatusSeq.AddChildren(checkDoorStatus);

            goToDoorSel.AddChildren(checkFrontDoorStatusSeq);
            goToDoorSel.AddChildren(checkBackDoorStatusSeq);

            hasGotMoneyInvert.AddChildren(gotMoney);

            stealSeq.AddChildren(hasGotMoneyInvert);
            stealSeq.AddChildren(goToDoorSel);
            stealSeq.AddChildren(goToDiamond);
            stealSeq.AddChildren(goToVan);

            _tree.AddChildren(stealSeq);
        }

        private ProcessStatusEnum GotMoney()
        {
            if(_money >= 500)
            {
                return ProcessStatusEnum.SUCCESS;
            }
            return ProcessStatusEnum.FAILED;
        }

        private ProcessStatusEnum GoToDiamond()
        {
            ProcessStatusEnum status = GenericActions.GoToItemToSteal(_itemsToSteal[_currentItemStealing + 1].gameObject, _itemsToSteal[_currentItemStealing + 1].Data.Type, this);
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
            ProcessStatusEnum status = GenericActions.GoToDestination(_van, this, ItemEnum.VAN);
            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                for (int i = 0; i < _itemsToSteal.Count; i++)
                {
                    _money += _collectedItems[i].Value;
                    _itemsToSteal[i].gameObject.SetActive(false);

                }
                _collectedItems.Clear();
                _itemsToSteal.Clear();
                _currentItemStealing = -1;
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
                _atCurrentDoor.SetActive(false);
                return doorStatus;
            }
            return doorStatus;
        }
    }
}
