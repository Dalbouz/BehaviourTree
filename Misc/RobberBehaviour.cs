using UnityEngine;
using Dawud.BT.General;
using UnityEngine.AI;
using Dawud.BT.Enums;
using Dawud.BT.Actions;
using System.Collections.Generic;
using Dawud.BT.Misc;

namespace Dawud.BT.Behaviour
{
    /// <summary>
    /// 
    /// </summary>
    public class RobberBehaviour : NPCMain
    {
        private int _currentItemStealing = -1;

        [SerializeField] private GameObject _van = default;
        [SerializeField] private GameObject _backDoor = default;
        [SerializeField] private GameObject _frontDoor = default;
        [SerializeField, Range(0, 1000)] private int _money = 800;
        [SerializeField] private List<ItemData> _collectedItems = new List<ItemData>();
        [SerializeField] private List<GameObject> _itemsToSteal = new List<GameObject>();

        private BehaviourTree _tree = default;

        void Start()
        {
            _currentItemStealing = -1;
            _collectedItems = new List<ItemData>();
            _itemsToSteal = new List<GameObject>();
            _itemsToSteal.AddRange(GenericActions.RandomAddingPickupableItemsToList());

            Agent = this.GetComponent<NavMeshAgent>();

            _tree = new BehaviourTree("Robber Tree Behaviour");
            Sequence steal = new Sequence("Steal something");
            Selector goToDoor = new Selector("Gos To Door");

            Leaf goToBackDoor = new Leaf("Go To back door", GoToBackDoor);
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);
            Leaf gotMoney = new Leaf("Got Money", GotMoney);

            goToDoor.AddChildren(goToFrontDoor);
            goToDoor.AddChildren(goToBackDoor);
            steal.AddChildren(gotMoney);
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
            if (_tree.Status.Equals(ProcessStatusEnum.AWAIT))
            {
                return;
            }

            if (!_tree.Status.Equals(ProcessStatusEnum.SUCCESS))
            {
                _tree.Status = _tree.Process();
                return;
            }
            if(_tree.Status.Equals(ProcessStatusEnum.SUCCESS) || _tree.Status.Equals(ProcessStatusEnum.FAILED))
            {
                _tree.SetAllToDefaultValues();
            }
        }

        private ProcessStatusEnum GotMoney()
        {
            if(_money >= 500)
            {
                return ProcessStatusEnum.FAILED;
            }
            return ProcessStatusEnum.SUCCESS;
        }

        private ProcessStatusEnum GoToDiamond()
        {
            ProcessStatusEnum status = GenericActions.GoToItemToSteal(_itemsToSteal[_currentItemStealing + 1], ItemEnum.DIAMOND, this);
            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                ItemData id = GenericActions.GetPickupableItemData(_itemsToSteal[_currentItemStealing + 1]);
                if (id != null)
                {
                    _collectedItems.Add(id); // Add to the list if the component exists
                }

                _currentItemStealing++;
            }
            return status;
        }

        private ProcessStatusEnum GoToVan()
        {
            ProcessStatusEnum status = GenericActions.GoToDestination(_van, this, ItemEnum.NONE);
            if (status.Equals(ProcessStatusEnum.SUCCESS))
            {
                for (int i = 0; i < _itemsToSteal.Count; i++)
                {
                    _money += _collectedItems[i].Value;
                    Destroy(_itemsToSteal[i]);

                }
                _collectedItems.Clear();
                _itemsToSteal.Clear();
                _currentItemStealing = -1;
            }
            return status;
        }

        private ProcessStatusEnum GoToFrontDoor()
        {
            return GenericActions.GoToDoor(_frontDoor, this);
        }

        private ProcessStatusEnum GoToBackDoor()
        {
            return GenericActions.GoToDoor(_backDoor, this);
        }
    }
}
