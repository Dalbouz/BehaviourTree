using UnityEngine;
using Dawud.BT.General;
using UnityEngine.AI;
using Dawud.BT.Enums;
using Dawud.BT.Actions;
using System;

namespace Dawud.BT.Behaviour
{
    /// <summary>
    /// 
    /// </summary>
    public class RobberBehaviour : NPCMain
    {
        [SerializeField] private GameObject _diamond = default;
        [SerializeField] private GameObject _van = default;
        [SerializeField] private GameObject _backDoor = default;
        [SerializeField] private GameObject _frontDoor = default;
        [SerializeField] BehaviourTree _tree = default;

        void Start()
        {
            Agent = this.GetComponent<NavMeshAgent>();

            _tree = new BehaviourTree("Robber Tree Behaviour");
            Sequence steal = new Sequence("Steal something");
            Selector goToDoor = new Selector("Go To Door");

            Leaf goToBackDoor = new Leaf("Go To back door", GoToBackDoor);
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            goToDoor.AddChildren(goToFrontDoor);
            goToDoor.AddChildren(goToBackDoor);
            steal.AddChildren(goToDoor);
            steal.AddChildren(goToDiamond);
            steal.AddChildren(goToVan);
            _tree.AddChildren(steal);

            if (_printOutTree)
            {
                _tree.PrintTree();
            }
            _tree.Status = ProcessStatusEnum.RUNNING;
        }

        private void Update()
        {
            if (_tree.Status.Equals(ProcessStatusEnum.RUNNING))
            {
                _tree.Status = _tree.Process();
                return;
            }
            if(_tree.Status.Equals(ProcessStatusEnum.SUCCESS) || _tree.Status.Equals(ProcessStatusEnum.FAILED))
            {
                _tree.SetAllToDefaultValues();
            }
        }

        private ProcessStatusEnum GoToDiamond()
        {
            return GoToItemToSteal(_diamond, ItemEnum.DIAMOND);
        }

        private ProcessStatusEnum GoToVan()
        {
            return GenericActions.GoToDestination(_van, this,ItemEnum.NONE);
        }

        private ProcessStatusEnum GoToFrontDoor()
        {
            return GoToDoor(_frontDoor);
        }

        private ProcessStatusEnum GoToBackDoor()
        {
            return GoToDoor(_backDoor);
        }

        private ProcessStatusEnum GoToDoor(GameObject door)
        {
            ProcessStatusEnum doorStatus = GenericActions.GoToDestination(door, this, ItemEnum.DOOR);
            if (doorStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                door.SetActive(false);
                return doorStatus;
            }
            return doorStatus;
        }

        private ProcessStatusEnum GoToItemToSteal(GameObject destinationObject, ItemEnum itemToSteal)
        {
            ProcessStatusEnum itemStatus = GenericActions.GoToDestination(destinationObject, this, itemToSteal);
            if (itemStatus.Equals(ProcessStatusEnum.SUCCESS))
            {
                destinationObject.SetActive(false);
                return itemStatus;
            }
            return itemStatus;
        }
    }
}
